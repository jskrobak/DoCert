using System.Globalization;
using System.Text;

namespace DoCert.Services;

public static class Extensions
{
    
    private static readonly string[] jednotky = {
        "", "jedna", "dva", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět"
    };

    private static readonly string[] desitky = {
        "", "deset", "dvacet", "třicet", "čtyřicet", "padesát",
        "šedesát", "sedmdesát", "osmdesát", "devadesát"
    };

    private static readonly string[] teen = {
        "deset", "jedenáct", "dvanáct", "třináct", "čtrnáct",
        "patnáct", "šestnáct", "sedmnáct", "osmnáct", "devatenáct"
    };

    private static readonly string[] stovky = {
        "", "sto", "dvě stě", "tři sta", "čtyři sta",
        "pět set", "šest set", "sedm set", "osm set", "devět set"
    };

    public static string ToWords(this int number)
    {
        if (number == 0)
            return "nula";

        if (number < 0)
            return "mínus " + ToWords(-number);

        string words = "";

        // Tisíce
        if (number >= 1000)
        {
            int tisice = number / 1000;
            number %= 1000;
            words += ConvertBelowThousand(tisice) + " tisíc ";
        }

        // Jednotky, desítky a stovky
        words += ConvertBelowThousand(number);

        return words.Trim();
    }

    private static string ConvertBelowThousand(int number)
    {
        string words = "";

        // Stovky
        if (number >= 100)
        {
            int sto = number / 100;
            number %= 100;
            words += stovky[sto] + " ";
        }

        // Desítky
        if (number >= 10 && number <= 19)
        {
            words += teen[number - 10] + " ";
        }
        else if (number >= 20)
        {
            int desitka = number / 10;
            number %= 10;
            words += desitky[desitka] + " ";
        }

        // Jednotky
        if (number > 0 && number < 10)
        {
            words += jednotky[number] + " ";
        }

        return words.Trim();
    }
    
}