using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Common
{
    /// <summary>
    /// 图片处理帮助类
    /// </summary>
    public class ImageHelper
    {
         /// <summary>
         /// 将图片路径转为二级制
         /// </summary>
         /// <param name="path">图片路径</param>
         /// <returns></returns>
        public static byte[] BufferStreamForByte(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中
            return imgBytesIn;
        }
    }
}
