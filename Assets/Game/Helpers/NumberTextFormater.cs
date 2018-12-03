using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class NumberTextFormater
{
    public static string FormatNumber(double number)
    {
        double check = 1;

        for (int i = 0; i < 9; i++)
        {
            if (number >= check && number < check * 1000)
            {
                string symbol = "";

                string leading = (number / check).ToString("F");

                switch (i)
                {
                    case 0:
                        leading = Math.Round(number / check).ToString();
                        break;
                    case 1:
                        symbol = "K";
                        break;
                    case 2:
                        symbol = "Mil";
                        break;
                    case 3:
                        symbol = "Bil";
                        break;
                    case 4:
                        symbol = "Tril";
                        break;
                    case 5:
                        symbol = "Quad";
                        break;
                    case 6:
                        symbol = "Quint";
                        break;
                    default:
                        break;
                }

                return leading + " " + symbol;
            }

            check = check * 1000;
        }

        return number.ToString();
    }
}