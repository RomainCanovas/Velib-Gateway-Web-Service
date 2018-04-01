# Rendu TP 2 - Velib Gateway Web Service

REST and SOAP WS Lab


### Extensions réalisées

-Interface graphique pour le client

-Cache

-Méthodes asynchrones (utilisées uniquement pour le client GUI)

### Comment ça marche?

Ouvrir le projet en ayant au préalablement lancé VS en mode administrateur.
Démarrer ConsoleApp ou GuiApp qui ouvrent respectivement le MVP
ou l'interface graphique. Comme précisé ci-dessus les
méthodes asynchrones ne sont appelées que dans la version GUI.
Enfin, le cache implémenté dans WcfVelib permet de stocker les réponses aux requêtes déjà traitées afin de les retourner sans faire une nouvelle requête.