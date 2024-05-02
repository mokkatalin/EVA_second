using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BombGame.Persistence
{
    public class TextFilePersistence : IPersistence
    {
        public FieldType[] Load(String path)
        {
            if (path == null) 
                throw new ArgumentNullException(nameof(path));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    StringBuilder input = new StringBuilder();
                    
                    while (!reader.EndOfStream)
                    {
                        input.Append(reader.ReadLine());
                        input.Append(' ');
                    }

                    String[] numbers = input.ToString().Split(' ');

                    FieldType[] values = new FieldType[numbers.Length];
    
                    for (Int32 i = 0; i < values.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(numbers[i])) //kivetel ha rossz a szam
                            values[i] = (FieldType)Int32.Parse(numbers[i]);
                    }

                    return values;              
                }
            }
            catch(IOException e)
            {
                throw new DataException("Error occurred during reading.");
            }
        }
    }
}
