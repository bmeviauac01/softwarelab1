# Szoftverfejlesztés laboratórium 1 feladatok

![Build docs](https://github.com/bmeviauac01/laborok/workflows/Build%20docs/badge.svg?branch=master)

[BMEVIAUAC09 Szoftverfejlesztés laboratórium 1](https://www.aut.bme.hu/Course/VIAUAC09/) tárgy laborfeladatai.

A jegyzetek MkDocs segítségével készülnek és GitHub Pages-en kerülnek publikálásra: <https://bmeviauac01.github.io/laborok/>

#### Helyi gépen történő renderelés (Docker-rel)

1. Powershell konzol nyitása a repository gyökerébe

1. `docker run -it --rm -p 8000:8000 -v ${PWD}:/docs squidfunk/mkdocs-material:7.1.5`

1. <http://localhost:8000> megnyitása böngészőből.

1. Markdown szerkesztése és mentése után automatikusan frissül a weboldal
