using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models
{
    public enum AgeGroups
    {
        Cubbies,
        Sparkies,
        TNT
    }

    public static class AgeGroupsExtensions
    {
        public static string ToAgeString(this AgeGroups group)
        {
            if (group.Equals(AgeGroups.Cubbies))
            {
                return "Pre-K";
            }
            else if (group.Equals(AgeGroups.Sparkies))
            {
                return "K-2";
            }
            else if (group.Equals(AgeGroups.TNT))
            {
                return "3-6";
            }

            throw new ArgumentException("Unknown Age Group provided.");
        }

        public static List<string> GetAgeStringList()
        {
            return new List<string> 
                { 
                    ToAgeString(AgeGroups.Cubbies),
                    ToAgeString(AgeGroups.Sparkies),
                    ToAgeString(AgeGroups.TNT)
                };
        }

        public static AgeGroups FromGrade(int grade)
        {
            if (grade == -1)
            {
                return AgeGroups.Cubbies;
            }
            else if (grade >= 0 && grade <= 2)
            {
                return AgeGroups.Sparkies;
            }
            else if (grade >= 3 && grade <= 6)
            {
                return AgeGroups.TNT;
            }

            throw new ArgumentOutOfRangeException("Invalid grade given");
        }
    }
}