using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteAutomatizadoSLPO
{
    class Verse
    {
        // Atributos
        private Dictionary<dynamic, dynamic> Node = new Dictionary<dynamic, dynamic>();

        // Public
        public Verse()
        {

        }

        public Verse(params dynamic[] i)
        {
            foreach (dynamic value in i)
            {
                int index = this.FreeIndex();
                this.Node[index] = value;
            }
        }

        public Verse(params KeyValuePair<dynamic, dynamic>[] i)
        {
            foreach (dynamic pair in i)
                this.Node[pair.Key] = pair.Value;
        }

        public void Set(params dynamic[] i)
        {
            if (i.Length < 2) // Erro
                this.Exception("Verse.Set, to few parameters.");

            else if (i.Length == 2)
            {
                dynamic index = i[0];

                if (index == null)
                    index = this.FreeIndex();

                this.Node[index] = i[1];
            }

            else
            {
                dynamic index = i[0];

                if (index == null)
                    index = this.FreeIndex();

                if (this.Node.ContainsKey(index) == false)
                    this.Node[index] = new Verse();

                if (this.Node[index] is Verse == false)
                    this.Node[index] = new Verse();

                dynamic[] subpath = new dynamic[i.Length - 1]; // removendo primeiro item do array
                Array.Copy(i, 1, subpath, 0, i.Length - 1);

                this.Node[index].Set(subpath);
            }
        }

        public dynamic Get(params dynamic[] i)
        {
            if (i.Length == 0) // Nodo é valor final
                return this.Node;

            else if (i.Length == 1) // Nodo é valor final
            {
                if (this.Node[i[0]] is Verse)
                    return this.Node[i[0]].Get();
                else return this.Node[i[0]];
            }

            else
            {
                if (this.Node.ContainsKey(i[0]) == false)
                    this.Exception("Verse.Get, unknow index " + i[0] + ".");

                if (this.Node[i[0]] is Verse)
                {
                    dynamic[] subpath = new dynamic[i.Length - 1]; // removendo primeiro item do array
                    Array.Copy(i, 1, subpath, 0, i.Length - 1);

                    return this.Node[i[0]].Get(subpath);
                }

                else
                    this.Exception("Verse.Get, unknow index " + i[0] + ".");

                return null;
            }
        }

        public void UnSet(params dynamic[] i)
        {
            if (i.Length <= 0)
                this.Exception("Verse.UnSet, to few parameters.");

            else if (i.Length == 1)
            {
                if (this.Node.ContainsKey(i[0]) == false)
                    this.Exception("Verse.UnSet, unknow index " + i[0] + ".");
                else this.Node.Remove(i[0]);
            }

            else
            {
                if (this.Node.ContainsKey(i[0]) == false)
                    this.Exception("Verse.UnSet, unknow index " + i[0] + ".");

                if (this.Node[i[0]] is Verse == false)
                    this.Exception("Verse.UnSet, index " + i[0] + " not an Verse.");

                dynamic[] subpath = new dynamic[i.Length - 1]; // removendo primeiro item do array
                Array.Copy(i, 1, subpath, 0, i.Length - 1);

                this.Node[i[0]].UnSet(subpath);
            }
        }

        public int GetFreeIndex(params dynamic[] i)
        {
            if (i.Length == 0)
                return this.FreeIndex();
            else
            {
                if (this.Node.ContainsKey(i[0]) == false)
                    this.Node[i[0]] = new Verse();

                if (this.Node[i[0]] is Verse == false)
                    this.Exception("Verse.GetFreeIndex, index " + i[0] + " not an Verse.");

                dynamic[] subpath = new dynamic[i.Length - 1]; // removendo primeiro item do array
                Array.Copy(i, 1, subpath, 0, i.Length - 1);

                return this.Node[i[0]].GetFreeIndex(subpath);
            }
        }

        public string print_r(int tabForce = 0)
        {
            string ret = "";

            string tab0 = ""; for (int i = 0; i < tabForce; i++) tab0 += "\t";
            string tab1 = ""; for (int i = 0; i <= tabForce; i++) tab1 += "\t";
            tabForce++;

            ret += "Verse\n";
            ret += tab0 + "{ \n";

            foreach (dynamic pair in this.Node)
                if (this.Node[pair.Key] is Verse)
                    ret += tab1 + pair.Key + " => " + this.Node[pair.Key].print_r(tabForce) + "\n";
                else ret += tab1 + pair.Key + " => " + pair.Value + "\n";

            ret += tab0 + "} \n";

            return ret;
        }

        public Boolean IsSet(params dynamic[] i)
        {
            if (i.Length == 0)
                return true;

            else if (i.Length == 1)
            {
                if (this.Node.ContainsKey(i[0]) == true)
                    return true;
                else return false;
            }

            else
            {
                if (this.Node.ContainsKey(i[0]) == false)
                    return false;

                if (this.Node[i[0]] is Verse)
                {
                    dynamic[] subpath = new dynamic[i.Length - 1]; // removendo primeiro item do array
                    Array.Copy(i, 1, subpath, 0, i.Length - 1);

                    return this.Node[i[0]].IsSet(subpath);
                }

                else return false;
            }
        }

        // Private
        private void Exception(string Msg)
        {
            Console.WriteLine(Msg);
            System.Environment.Exit(0xff);
        }

        private int FreeIndex()
        {
            int index = 0;
            while (true)
            {
                if (this.Node.ContainsKey(index) == false) break;
                if (index == 2147483647) // https://msdn.microsoft.com/en-us/library/exx3b86w.aspx
                    this.Exception("Verse: No more indexes avaible");
                index++;
            }
            return index;
        }
    }
}
