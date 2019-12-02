using System;
using System.Collections.Generic;
using System.Linq;
using ahk.adatvez.mssqldb.DatabaseContext;

namespace ahk.adatvez.mssqldb
{
    public static class DbSampleData
    {
        public static Termek InsertSampleProduct(string name = null)
        {
            using (var db = DbFactory.GetDatabase())
            {
                var value = CreateNewProduct(name);
                db.Termek.Add(value);
                db.SaveChanges();
                return value;
            }
        }

        public static Termek CreateNewProduct(string name = null)
        {
            return new Termek()
            {
                Nev = name ?? Guid.NewGuid().ToString(),
                NettoAr = 45431,
                Afaid = 3,
                KategoriaId = 1,
                Raktarkeszlet = 991
            };
        }

        public static Termek GetProduct(int id)
        {
            using (var db = DbFactory.GetDatabase())
                return db.Termek.FirstOrDefault(x => x.Id == id);
        }

        public static IReadOnlyCollection<string> ListAllProductNames()
        {
            using (var db = DbFactory.GetDatabase())
                return db.Termek.Select(t => t.Nev).ToArray();
        }

        public static IReadOnlyCollection<int> ListAllAfaKulcs()
        {
            using (var db = DbFactory.GetDatabase())
                return db.Afa.Select(t => t.Kulcs.Value).ToArray();
        }

        public static Szamla InsertSampleSzamla()
        {
            using (var db = DbFactory.GetDatabase())
            {
                var value = new Szamla()
                {
                    FizetesiHatarido = DateTime.UtcNow.AddDays(10),
                    FizetesiMod = "készpénz",
                    KiallitasDatum = DateTime.UtcNow,
                    Kiallito = db.SzamlaKiallito.First(),
                    Megrendeles = db.Megrendeles.First(),
                    MegrendeloIr = "111",
                    MegrendeloNev = "Kkk Jjj",
                    MegrendeloUtca = "QWwew",
                    MegrendeloVaros = "asas asasa",
                    NyomtatottPeldanyszam = 1,
                    Sztorno = false,
                    TeljesitesDatum = null,
                    SzamlaTetel = new[]
                {
                        new SzamlaTetel()
                        {
                            Afakulcs = 27,
                            Mennyiseg = DateTime.UtcNow.Second,
                            NettoAr = 1122,
                            Nev = "Piros alma",
                        },
                        new SzamlaTetel()
                        {
                            Afakulcs = 27,
                            Mennyiseg = DateTime.UtcNow.Minute,
                            NettoAr = 99,
                            Nev = "Piros korte",
                        }
                    }
                };

                db.Szamla.Add(value);
                db.SaveChanges();
                return value;
            }
        }
    }
}
