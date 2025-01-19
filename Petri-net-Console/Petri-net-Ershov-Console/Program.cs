namespace Petri_net_Ershov_Console
{
    internal class Program
    {
        static Random rnd = new Random();
        static int[] inputs, In, Out, outputs, Places;
        static int n;   // places num / количество мест 
        static int m;   // transitions num / количество переходов

        static void Main(string[] args)
        {
            Init();

            RunPetriNet(T: 5, p0: 0.4);

            Console.ReadKey();
        }

        static void Init()
        {
            Console.WriteLine("Initialization: n, m, inputs[], In[], Out[], outputs[]");

            n = 9; // places num / количество мест 
            m = 4; // transitions num / количество переходов

            inputs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            In = new int[] { 0, 3, 6, 7 };
            outputs = new int[] { 3, 4, 5, 6, 7, 8 };
            Out = new int[] { 0, 3, 4, 5 };

            n = 6; // places num / количество мест 
            m = 2; // transitions num / количество переходов

            inputs = new int[] { 0, 1, 2, 3, 4 };
            In = new int[] { 0, 3 };
            outputs = new int[] { 3, 4, 5 };
            Out = new int[] { 0, 2 };


            Places = new int[n];

            for (int i = 0; i < n; i++)
            {
                if (rnd.NextDouble() < 0.4)
                {
                    Places[i] = 1;
                }
                else
                {
                    Places[i] = 0;
                }

                Places[i] = rnd.Next(2); // random 0 or 1
            }

            Console.WriteLine("\nn / places num = " + n);
            Console.WriteLine("m / transitions num = " + m);

            Console.WriteLine("\ninputs[] / input places array: " + ToString(inputs));
            Console.WriteLine("outputs[]/ output places array:" + ToString(outputs));

            Console.WriteLine("\nIn[] / start indexes of input places: " + ToString(In));
            Console.WriteLine("Out[]/ start indexes of output places:" + ToString(Out));

            Console.WriteLine("\nPlaces[] / tokens array: " + ToString(Places));
            Console.WriteLine("Petri Net: " + PetriToString());
        }

        static void RunPetriNet(int T, double p0)
        {
            for (int t = 1; t <= T; t++)
            {
                Console.WriteLine("\nPETRI NET UPDATING....... / t = " + t);

                UpdatePetriNet();

                Console.WriteLine("Places[] / tokens array: " + ToString(Places));
                Console.WriteLine("Petri Net: " + PetriToString());
            }
        }

        static void UpdatePetriNet()
        {
            for (int j = 0; j < m; j++) // m - transitions num
            {
                UpdateTransition(j);
            }

            for (int i = 0; i < n; i++) // n - places num
            {
                /*
                 *  Place[i]:
                 *  0 - place free, 
                 *  1 - token in the place, 
                 *  2 - place should be free, 
                 *  3 - plae rezerved
                 */
                if (Places[i] > 1)
                {
                    Places[i] = Places[i] - 2;
                }
            }
        }

        static void UpdateTransition(int j)
        {
            int i_in = -1;
            int i_out = -1;

            int start = In[j];
            int end = GetEndIndex(In, inputs, start, j);

            // among the input places, the place occupied by the token is searched in descending order.
            // среди входных мест ищется место, занятое машиной, по убыванию
            for (int i = start; i <= end; ++i)
            {
                if (Places[inputs[i]] == 1 && i_in == -1)
                {
                    i_in = inputs[i];
                }
            }


            // Shuffle
            start = Out[j];
            end = GetEndIndex(Out, outputs, start, j);
            Shuffle(start, end);

            // We are looking for a free place among the output places
            // Среди выходных мест ищется свободное место

            start = Out[j];
            end = GetEndIndex(Out, outputs, start, j);

            for (int i = start; i <= end; ++i)
            {
                if (Places[outputs[i]] == 0 && i_out == -1)
                {
                    i_out = outputs[i];
                }
            }

            // Deferred transfer / Отложенный перенос
            if (i_in != -1 && i_out != -1)
            {
                Places[i_in] = 2;
                Places[i_out] = 3;
            }
        }

        static int GetEndIndex(int[] pereh, int[] mesto, int start, int start_indx)
        {
            if (start_indx >= pereh.Length) return start;

            if (start_indx < pereh.Length - 1)
            {
                int val = pereh[start_indx + 1];
                return val - 1;
            }
                
            else
                return mesto.Length - 1;
        }

        static void Shuffle(int start, int end)
        {
            for (int i = outputs[start]; i <= outputs[end]; ++i)
            {
                int j = rnd.Next(i, i + 1);
                var temp = Places[i];
                Places[i] = Places[j];
                Places[j] = temp;
            }
        }

        static string ToString(int[] arr)
        {
            string s = "[";
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i] >= 0.0) s += " ";
                s += arr[i].ToString("F0") + " ";
            }
            s += "]";
            return s;
        }

        static string PetriToString()
        {
            string s = "[ ";

            for (int j = 0; j < m; j++) // m - transitions num
            {
                // draw inputs
                int start = In[j];
                int end = GetEndIndex(In, inputs, start, j);
                for (int i = inputs[start]; i <= inputs[end]; ++i)
                {
                    if(j == 0)
                        s += Places[i].ToString("F0") + " ";
                }

                // draw perehod
                s += " -> ";

                // draw outputs
                start = Out[j];
                end = GetEndIndex(Out, outputs, start, j);
                for (int i = outputs[start]; i <= outputs[end]; ++i)
                {
                    s += Places[i].ToString("F0") + " ";
                }
            }
            s += "]";
            return s;
        }
    }
}
