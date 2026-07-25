using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Keyword
{
    public enum GeneralEmotion
    {
        Joy,
        Anger,
        Sadness
    }

    public enum SpecialEmotionType
    {
        Fear,
        Pain,
        Collapse
    }

    public struct SpecialEmotionData
    {
        public string Id;
        public string NameKey;
        public string DescKey;
        public int EmotionLevel;
        public bool IsAwakening;
        public bool IsCollapse;
        public bool AutoRemovable;
        public bool IsReplaced;
        public List<string> EffectKeys;
        public int TurnEndSanityLoss;
        public string ConvertToEmotionId;

        public string Name => LocalizationManager.Get(NameKey);
        public string Description => LocalizationManager.Get(DescKey);
        public List<string> Effects => EffectKeys.Select(k => LocalizationManager.Get(k)).ToList();

        public string TurnEndDescription
        {
            get
            {
                var parts = new List<string>();
                if (TurnEndSanityLoss > 0)
                    parts.Add(LocalizationManager.Get("special_emotion_turn_end_sanity_loss")
                        .Replace("{0}", TurnEndSanityLoss.ToString()));
                if (!string.IsNullOrEmpty(ConvertToEmotionId) && SpecialEmotionDB.All.TryGetValue(ConvertToEmotionId, out var target))
                    parts.Add(string.Format(LocalizationManager.Get("special_emotion_turn_end_convert"), target.Name));
                return parts.Count > 0 ? string.Join("；", parts) : "";
            }
        }
    }

    public static class SpecialEmotionDB
    {
        private static readonly List<string> FearEffectKeys = new()
        {
            "special_emotion_fear_effect_0",
            "special_emotion_fear_effect_1",
            "special_emotion_fear_effect_2",
            "special_emotion_fear_effect_3",
            "special_emotion_fear_effect_4",
            "special_emotion_fear_effect_5",
            "special_emotion_fear_effect_6",
            "special_emotion_fear_effect_7",
            "special_emotion_fear_effect_8",
        };

        private static readonly List<string> PainEffectKeys = new()
        {
            "special_emotion_pain_effect_0",
            "special_emotion_pain_effect_1",
            "special_emotion_pain_effect_2",
            "special_emotion_pain_effect_3",
            "special_emotion_pain_effect_4",
            "special_emotion_pain_effect_5",
            "special_emotion_pain_effect_6",
            "special_emotion_pain_effect_7",
            "special_emotion_pain_effect_8",
            "special_emotion_pain_effect_9",
            "special_emotion_pain_effect_10",
            "special_emotion_pain_effect_11",
            "special_emotion_pain_effect_12",
            "special_emotion_pain_effect_13",
            "special_emotion_pain_effect_14",
            "special_emotion_pain_effect_15",
            "special_emotion_pain_effect_16",
        };

        private static readonly List<string> CollapseEffectKeys = new()
        {
            "special_emotion_collapse_effect_0",
            "special_emotion_collapse_effect_1",
            "special_emotion_collapse_effect_2",
            "special_emotion_collapse_effect_3",
            "special_emotion_collapse_effect_4",
            "special_emotion_collapse_effect_5",
            "special_emotion_collapse_effect_6",
            "special_emotion_collapse_effect_7",
            "special_emotion_collapse_effect_8",
            "special_emotion_collapse_effect_9",
            "special_emotion_collapse_effect_10",
            "special_emotion_collapse_effect_11",
            "special_emotion_collapse_effect_12",
            "special_emotion_collapse_effect_13",
            "special_emotion_collapse_effect_14",
            "special_emotion_collapse_effect_15",
            "special_emotion_collapse_effect_16",
            "special_emotion_collapse_effect_17",
        };

        public static readonly SpecialEmotionData Fear = new()
        {
            Id = "fear",
            NameKey = "special_emotion_fear",
            DescKey = "special_emotion_fear_desc",
            EmotionLevel = 1,
            IsAwakening = false,
            IsCollapse = true,
            AutoRemovable = true,
            IsReplaced = true,
            EffectKeys = FearEffectKeys,
            TurnEndSanityLoss = 5,
            ConvertToEmotionId = null,
        };

        public static readonly SpecialEmotionData Pain = new()
        {
            Id = "pain",
            NameKey = "special_emotion_pain",
            DescKey = "special_emotion_pain_desc",
            EmotionLevel = 2,
            IsAwakening = false,
            IsCollapse = true,
            AutoRemovable = false,
            IsReplaced = false,
            EffectKeys = PainEffectKeys,
            TurnEndSanityLoss = 10,
            ConvertToEmotionId = "fear",
        };

        public static readonly SpecialEmotionData Collapse = new()
        {
            Id = "collapse",
            NameKey = "special_emotion_collapse",
            DescKey = "special_emotion_collapse_desc",
            EmotionLevel = 3,
            IsAwakening = false,
            IsCollapse = true,
            AutoRemovable = false,
            IsReplaced = false,
            EffectKeys = CollapseEffectKeys,
            TurnEndSanityLoss = 15,
            ConvertToEmotionId = "pain",
        };

        public static readonly Dictionary<string, SpecialEmotionData> All = new()
        {
            { "fear", Fear },
            { "pain", Pain },
            { "collapse", Collapse },
        };

        public static bool IsCollapseEmotion(string name) =>
            name == "fear" || name == "pain" || name == "collapse";

        public static bool IsAwakeningEmotion(GeneralEmotion emotion) =>
            emotion == GeneralEmotion.Joy ||
            emotion == GeneralEmotion.Anger ||
            emotion == GeneralEmotion.Sadness;
    }
}
