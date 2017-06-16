using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotions
{
    public class ServiceEmotions
    {
        static string key = "fdde36d2ec67462285dbf6ca98a5db3b";
        public static async Task<Dictionary<string, float>> GetEmotions(Stream stream)
        {
            EmotionServiceClient cliente = new EmotionServiceClient(key);
            var emotions = await cliente.RecognizeAsync(stream);
            if (emotions == null || emotions.Count() == 0)
                return null;
            return emotions[0].Scores.ToRankedList().ToDictionary(x => x.Key, x =>
            x.Value);
        }
    }
}

