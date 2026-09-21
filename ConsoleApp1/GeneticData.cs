using System;

namespace ConsoleApp1
{
    public struct GeneticData
    {
        public string protein;
        public string organism;
        public string amino_acids;

        public GeneticData(string targetProtein, string targetOrganism, string rawAminoAcids)
        {
            protein = targetProtein;
            organism = targetOrganism;
            amino_acids = rawAminoAcids;
        }
    }
}
