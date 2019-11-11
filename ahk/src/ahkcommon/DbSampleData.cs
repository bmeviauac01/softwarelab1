using adatvez.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace adatvez
{
    public static class DbSampleData
    {
        public static Termek InsertSampleProduct(string name = null)
        {
            using (var db = DbFactory.GetDatabase())
            {
                var value = new Termek()
                {
                    Nev = name ?? Guid.NewGuid().ToString(),
                    NettoAr = 45431,
                    Afaid = 3,
                    KategoriaId = 1,
                    Raktarkeszlet = 991
                };
                db.Termek.Add(value);
                db.SaveChanges();
                return value;
            }
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
    }
}
