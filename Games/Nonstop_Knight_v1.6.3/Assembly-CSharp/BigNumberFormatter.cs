using System;
using System.Collections.Generic;
using System.Globalization;

public class BigNumberFormatter : IFormatProvider, ICustomFormatter
{
    public const double POSTFIX_INCREMENT_EXPONENT = 3.0;
    public static List<string> POSTFIXES;

    static BigNumberFormatter()
    {
        List<string> list = new List<string>();
        list.Add(string.Empty);
        list.Add("K");
        list.Add("M");
        list.Add("B");
        list.Add("T");
        list.Add("aa");
        list.Add("bb");
        list.Add("cc");
        list.Add("dd");
        list.Add("ee");
        list.Add("ff");
        list.Add("gg");
        list.Add("hh");
        list.Add("ii");
        list.Add("jj");
        list.Add("kk");
        list.Add("ll");
        list.Add("mm");
        list.Add("nn");
        list.Add("oo");
        list.Add("pp");
        list.Add("qq");
        list.Add("rr");
        list.Add("ss");
        list.Add("tt");
        list.Add("uu");
        list.Add("vv");
        list.Add("ww");
        list.Add("xx");
        list.Add("yy");
        list.Add("zz");
        list.Add("AA");
        list.Add("BB");
        list.Add("CC");
        list.Add("DD");
        list.Add("EE");
        list.Add("FF");
        list.Add("GG");
        list.Add("HH");
        list.Add("II");
        list.Add("JJ");
        list.Add("KK");
        list.Add("LL");
        list.Add("MM");
        list.Add("NN");
        list.Add("OO");
        list.Add("PP");
        list.Add("QQ");
        list.Add("RR");
        list.Add("SS");
        list.Add("TT");
        list.Add("UU");
        list.Add("VV");
        list.Add("WW");
        list.Add("XX");
        list.Add("YY");
        list.Add("ZZ");
        list.Add("aaa");
        list.Add("bbb");
        list.Add("ccc");
        list.Add("ddd");
        list.Add("eee");
        list.Add("fff");
        list.Add("ggg");
        list.Add("hhh");
        list.Add("iii");
        list.Add("jjj");
        list.Add("kkk");
        list.Add("lll");
        list.Add("mmm");
        list.Add("nnn");
        list.Add("ooo");
        list.Add("ppp");
        list.Add("qqq");
        list.Add("rrr");
        list.Add("sss");
        list.Add("ttt");
        list.Add("uuu");
        list.Add("vvv");
        list.Add("www");
        list.Add("xxx");
        list.Add("yyy");
        list.Add("zzz");
        list.Add("AAA");
        list.Add("BBB");
        list.Add("CCC");
        list.Add("DDD");
        list.Add("EEE");
        list.Add("FFF");
        list.Add("GGG");
        list.Add("HHH");
        list.Add("III");
        list.Add("JJJ");
        list.Add("KKK");
        list.Add("LLL");
        list.Add("MMM");
        list.Add("NNN");
        list.Add("OOO");
        list.Add("PPP");
        list.Add("QQQ");
        list.Add("RRR");
        list.Add("SSS");
        list.Add("TTT");
        list.Add("UUU");
        list.Add("VVV");
        list.Add("WWW");
        list.Add("XXX");
        list.Add("YYY");
        list.Add("ZZZ");
        POSTFIXES = list;
    }

    public string Format(string fmt, object arg, IFormatProvider formatProvider)
    {
        double d = (double) arg;
        double num2 = (d <= 0.0) ? 0.0 : Math.Log10(d);
        int num3 = (int) Math.Floor(num2 / 3.0);
        if (num3 <= 0)
        {
            return d.ToString("0");
        }
        if (num3 >= POSTFIXES.Count)
        {
            return d.ToString("000.00e+0", CultureInfo.InvariantCulture);
        }
        double num4 = Math.Pow(10.0, num3 * 3.0);
        string str = (d / num4).ToString("###.00");
        switch (str.IndexOf("."))
        {
            case 1:
            case 2:
                str = str.Substring(0, 4);
                break;

            case 3:
                str = str.Substring(0, 3);
                break;
        }
        return (str + POSTFIXES[num3]);
    }

    public object GetFormat(Type formatType)
    {
        if (formatType == typeof(ICustomFormatter))
        {
            return this;
        }
        return null;
    }
}

