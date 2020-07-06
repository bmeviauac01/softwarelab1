# Szoftverfejlesztés laboratórium 1 feladatok

![Build docs](https://github.com/bmeviauac01/laborok/workflows/Build%20docs/badge.svg?branch=master)

[BMEVIAUAC09 Szoftverfejlesztés laboratórium 1](https://www.aut.bme.hu/Course/VIAUAC09/) tárgy laborfeladatai.

A jegyzetek MkDocs segítségével készülnek és GitHub Pages-en kerülnek publikálásra: <https://bmeviauac01.github.io/laborok/>

#### Helyi gépen történő renderelés

1. Powershell konzol nyitása a repository gyökerébe

1. `docker run -it --rm -p 8000:8000 -v ${PWD}:/src --workdir /src python:3.8-slim /bin/bash -c "pip install -r requirements_docs.txt;mkdocs serve --dev-addr=0.0.0.0:8000"`

1. <http://localhost:8000> megnyitása böngészőből.

1. Markdown szerkesztése és mentése után automatikusan frissül a weboldal
