using System.Collections.Generic;
using GodotEngine;

namespace ProjectRA
{
    /// <summary>
    /// 本地化管理器。从 JSON 文件加载字符串表，Get(key) 按当前语言返回文本。
    /// 添加新语言：在 Assets/Localization/ 下放置 JSON 文件，调用 SetLocale() 即可切换。
    /// </summary>
    public static class LocalizationManager
    {
        /// <summary>key → 翻译文本 的字典</summary>
        private static Dictionary<string, string> _strings = new();

        /// <summary>当前语言代码，如 "zh-CN"</summary>
        public static string CurrentLocale { get; private set; } = "zh-CN";

        /// <summary>从 JSON 字符串加载指定语言的数据</summary>
        /// <param name="localeCode">语言代码，例如 "zh-CN" / "en"</param>
        /// <param name="jsonContent">JSON 文本内容</param>
        public static void LoadFromJson(string localeCode, string jsonContent)
        {
            var parsed = JsonUtility.FromJson<LocalizationData>(jsonContent);
            if (parsed?.entries == null) return;

            _strings = new Dictionary<string, string>();
            foreach (var entry in parsed.entries)
                _strings[entry.key] = entry.value;

            CurrentLocale = localeCode;
        }

        /// <summary>切换语言，等价于 LoadFromJson</summary>
        public static void SetLocale(string localeCode, string jsonContent)
        {
            LoadFromJson(localeCode, jsonContent);
        }

        /// <summary>获取 key 对应的翻译文本。若 key 不存在则返回 key 本身作为回退</summary>
        public static string Get(string key)
        {
            if (_strings.TryGetValue(key, out var value))
                return value;
            return key;
        }

        /// <summary>获取翻译文本并格式化（string.Format）</summary>
        public static string Get(string key, params object[] args)
        {
            var template = Get(key);
            return string.Format(template, args);
        }

        [System.Serializable]
        private class LocalizationData
        {
            public LocalizationEntry[] entries;
        }

        [System.Serializable]
        private class LocalizationEntry
        {
            public string key;
            public string value;
        }
    }
}
