using DBLayer.Interface;

namespace DBLayer.Persistence.Generator;

public class DUIDGenerator : IGenerator
{
    public object Generate()
    {
        var buffer = Guid.NewGuid().ToByteArray();
        var longGuid = BitConverter.ToInt64(buffer, 0);

        var value = Math.Abs(longGuid).ToString();

        var buf = new byte[value.Length];
        var p = 0;
        for (var i = 0; i < value.Length;)
        {
            var ph = Convert.ToByte(value[i]);

            var fix = 1;
            if ((i + 1) < value.Length)
            {
                var pl = Convert.ToByte(value[i + 1]);
                buf[p] = (byte)((ph << 4) + pl);
                fix = 2;
            }
            else
            {
                buf[p] = (ph);
            }

            if ((i + 3) < value.Length)
            {
                if (Convert.ToInt16(value.Substring(i, 3)) < 256)
                {
                    buf[p] = Convert.ToByte(value.Substring(i, 3));
                    fix = 3;
                }
            }
            p++;
            i = i + fix;
        }
        var buf2 = new byte[p];
        for (var i = 0; i < p; i++)
        {
            buf2[i] = buf[i];
        }
        string guid2Int = BitConverter.ToInt32(buf2, 0).ToString().Replace("-", "").Replace("+", "");
        guid2Int = guid2Int.Length >= 9 ? guid2Int.Substring(0, 9) : guid2Int.PadLeft(9, '0');
        return Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmm") + guid2Int);
    }
}
