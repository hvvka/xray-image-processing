using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace XRayImageProcessing.Models.Processors
{
    public class TumorFloodFill : IProcessor
    {
        public TumorFloodFill(List<int> colors)
        {
            Colors = colors;
        }

        public List<int> Colors { get; }

        public int[] Process(int[] data, int width, int height)
        {
            if (Colors.Count == 0) return data; // there are two tumors initially
            Colors.Sort();
            return FloodFiller.FillEverythingExceptColors(data, width, height, Colors, Color.Transparent.ToArgb());
        }

        public static List<int> getMostFrequentColors(int[] data)
        {
            return data.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).ToList();
        
        }
    }
}
