using adatvez.DAL.Entities;
using ahk.common.Helpers;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace adatvez
{
    public class RandomEntityFactory
    {
        public static Termek CreateRandomTermek()
            => new Termek
            {
                KategoriaID = ObjectId.Parse("5d7e42adcffa8e1b64f7dbce"),
                Nev = Guid.NewGuid().ToString(),
                NettoAr = RandomHelper.GetRandomValue(4000, 7500),
                Raktarkeszlet = 56,
            };

        public static Kategoria CreateRandomKategoria() => new Kategoria { Nev = Guid.NewGuid().ToString() };

        public static Telephely CreateRandomTelephely()
            => new Telephely
            {
                ID = ObjectId.GenerateNewId(),
                IR = RandomHelper.GetRandomValue(1000, 9999).ToString(),
                Utca = Guid.NewGuid().ToString(),
                Varos = Guid.NewGuid().ToString(),
            };

        public static Vevo CreateRandomVevo(int telephelyCount)
        {
            var telephelyek = Enumerable.Range(0, telephelyCount)
                .Select(_ => CreateRandomTelephely())
                .ToArray();

            return new Vevo
            {
                Nev = Guid.NewGuid().ToString(),
                KozpontiTelephelyID = telephelyek.Last().ID,
                Telephelyek = telephelyek,
            };
        }

        public static MegrendelesTetel CreateRandomMegrendelesTetel(Termek termek, string statusz)
            => new MegrendelesTetel
            {
                TermekID = termek.ID,
                Statusz = statusz,
                Mennyiseg = RandomHelper.GetRandomValue(30, 50),
                NettoAr = termek.NettoAr,
            };

        public static Megrendeles CreateRandomMegrendeles(ObjectId vevoId, ObjectId telephelyId, DateTime datum, params Termek[] termekek)
        {
            var statusz = Guid.NewGuid().ToString();
            var megrendelesTetelek = termekek
                .Select(termek => CreateRandomMegrendelesTetel(termek, statusz))
                .ToArray();

            return new Megrendeles
            {
                VevoID = vevoId,
                TelephelyID = telephelyId,
                Statusz = statusz,
                FizetesMod = new FizetesMod { Mod = Guid.NewGuid().ToString(), Hatarido = 999 },
                Datum = datum,
                Hatarido = datum.AddDays(RandomHelper.GetRandomValue(7, 14)),
                MegrendelesTetelek = megrendelesTetelek,
            };
        }
    }
}
