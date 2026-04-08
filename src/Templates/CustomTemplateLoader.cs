using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PostfixTemplates.Templates
{
    /// <summary>
    /// Discovers, loads, and watches a <c>.postfix.json</c> file at the solution root
    /// to provide user-defined postfix templates.
    /// </summary>
    internal static class CustomTemplateLoader
    {
        /// <summary>
        /// The well-known file name for custom template definitions.
        /// </summary>
        public const string FileName = ".postfix.json";

        private static readonly object _lock = new object();
        private static IReadOnlyList<CustomTemplate> _templates = [];
        private static FileSystemWatcher _watcher;
        private static string _watchedFilePath;

        /// <summary>
        /// The currently loaded custom templates. Empty when no file is found
        /// or the file is invalid.
        /// </summary>
        public static IReadOnlyList<CustomTemplate> Templates
        {
            get
            {
                lock (_lock)
                {
                    return _templates;
                }
            }
        }

        /// <summary>
        /// Initializes the loader for the given solution directory. Loads the
        /// templates immediately and starts watching for file changes.
        /// </summary>
        public static void Initialize(string solutionDirectory)
        {
            StopWatching();

            if (string.IsNullOrEmpty(solutionDirectory))
            {
                return;
            }

            var filePath = Path.Combine(solutionDirectory, FileName);
            _watchedFilePath = filePath;

            LoadFromFile(filePath);
            StartWatching(solutionDirectory);
        }

        /// <summary>
        /// Clears loaded templates and stops the file watcher.
        /// Called when the solution closes.
        /// </summary>
        public static void Clear()
        {
            StopWatching();

            lock (_lock)
            {
                _templates = Array.Empty<CustomTemplate>();
            }
        }

        /// <summary>
        /// Parses custom template definitions from JSON content.
        /// Returns an empty list if the content is null, empty, or invalid.
        /// </summary>
        internal static IReadOnlyList<CustomTemplate> ParseJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<CustomTemplate>();
            }

            try
            {
                CustomTemplateFile file = JsonConvert.DeserializeObject<CustomTemplateFile>(json);

                if (file?.Templates == null || file.Templates.Length == 0)
                {
                    return Array.Empty<CustomTemplate>();
                }

                return file.Templates
                    .Where(d => !string.IsNullOrWhiteSpace(d.Name) && !string.IsNullOrWhiteSpace(d.Body))
                    .Select(d => new CustomTemplate(d))
                    .ToList();
            }
            catch (JsonException)
            {
                return Array.Empty<CustomTemplate>();
            }
        }

        private static void LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    lock (_lock)
                    {
                        _templates = Array.Empty<CustomTemplate>();
                    }

                    return;
                }

                var json = File.ReadAllText(filePath);
                IReadOnlyList<CustomTemplate> parsed = ParseJson(json);

                lock (_lock)
                {
                    _templates = parsed;
                }
            }
            catch (Exception ex)
            {
                ex.LogAsync().FireAndForget();

                lock (_lock)
                {
                    _templates = Array.Empty<CustomTemplate>();
                }
            }
        }

        private static void StartWatching(string directory)
        {
            try
            {
                _watcher = new FileSystemWatcher(directory, FileName)
                {
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                    EnableRaisingEvents = true
                };

                _watcher.Changed += OnFileChanged;
                _watcher.Created += OnFileChanged;
                _watcher.Deleted += OnFileChanged;
                _watcher.Renamed += OnFileRenamed;
            }
            catch (Exception ex)
            {
                ex.LogAsync().FireAndForget();
            }
        }

        private static void StopWatching()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Changed -= OnFileChanged;
                _watcher.Created -= OnFileChanged;
                _watcher.Deleted -= OnFileChanged;
                _watcher.Renamed -= OnFileRenamed;
                _watcher.Dispose();
                _watcher = null;
            }

            _watchedFilePath = null;
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            ReloadFromWatchedPath();
        }

        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            ReloadFromWatchedPath();
        }

        private static void ReloadFromWatchedPath()
        {
            var path = _watchedFilePath;

            if (!string.IsNullOrEmpty(path))
            {
                LoadFromFile(path);
            }
        }
    }
}
