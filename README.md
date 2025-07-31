📚 BibliothèqueSimplon API

Bienvenue dans le projet BibliothèqueSimplon !
Cette solution vise à digitaliser la gestion des livres, des membres, des emprunts et des employés pour une bibliothèque 👩‍🏫.
🚀 Contexte

Vous êtes développeur backend dans une start-up tech.
Le client souhaite remplacer son système manuel par une solution moderne et automatisée.
Votre mission : concevoir une API REST sécurisée, intuitive et performante.
🛠️ Stack technique
Composant	Technologie
Base de données	PostgreSQL 🐘
Backend	ASP.NET Core + EF Core ⚙️
Authentification	JWT 🔐
Interface admin	Razor Pages 🧑‍💼
Interface membre	React ⚛️
📦 Fonctionnalités
📕 Livres

    🔍 Consulter les livres

    ➕ Ajouter un livre

    ✏️ Modifier un livre

    ❌ Supprimer un livre

    ✅ Vérifier disponibilité

    📂 Champs : Titre, Auteur, ISBN, Disponible, Catégorie (enum JSON), État (enum JSON)

👥 Membres

    🔍 Consulter les membres

    ➕ Ajouter un membre

    ✏️ Modifier un membre

    ❌ Supprimer un membre

    🕓 Consulter l'historique (bonus)

🔄 Emprunts

    📦 Créer un emprunt avec plusieurs livres

    📅 Suivre date d’emprunt & date de retour prévue

    ⛔ Vérifier disponibilité avant l'emprunt

    👀 Voir les emprunts en cours et l’historique par membre

🧑‍💼 Employés

    🔐 Connexion sécurisée (JWT)

    🎭 Rôles : Administrateur, Bibliothécaire

    🔄 Réinitialisation de mot de passe (bonus)

    🎛️ Gestion :

        Livres

        Membres

        Emprunts

        Réservations

🔒 Sécurité

    Authentification via JWT

    Autorisation par rôle (admin / bibliothécaire)

    Routes sécurisées selon les permissions

🧱 Architecture

    MVC (Modèle / Vue / Contrôleur)

    DTOs pour isoler les entités des vues API

    Middleware de logging intégré 🧾

    Services injectés via DI

🔗 Relations et MCD (Modèle Conceptuel des Données)

Voici un aperçu des entités et de leurs relations :

Employé ──(1,1)── a un rôle (enum)
Employé ──(0,n)── gère ──> Livres / Emprunts / Réservations

Livre ──(0,n)── est emprunté dans ──> Emprunt (via LivreEmprunt)
Livre ──(0,n)── est réservé par ──> Réservation
Livre ◄── a ── Catégorie (enum JSON)
Livre ◄── a ── État (enum JSON)

Membre ──(1,n)── effectue ──> Emprunt
Membre ──(0,n)── fait ──> Réservation

🧠 Diagramme illustré disponible dans /docs/mcd-schema.png
✅ Exigences techniques

    ✅ MVC structuré

    ✅ Injection de dépendances

    ✅ DTOs

    ✅ EF Core (code-first)

    ✅ JWT

    ✅ Swagger intégré pour tester l’API


    🚀 Installation
Cloner le repo

git clone https: git clone https://github.com/Mister-deal/bibliothequeSimplon.git
cd bibliotheque-simplon

Installer Dotnet EF de manière globale dans votre ordinateur ouvrez votre powershell ou CMD en administrateur et ajoutez cette ligne

# Configure ton appsettings.json (PostgreSQL, JWT secret, etc.)

dotnet tool install --global dotnet -ef

**créer un fichier .txt nommé "db-password" pour lier votre bdd et votre projet avec le nom du mdp de votre UserAdmin dans postgresql

Creer la migration dans la console de la solution

dotnet ef migrations add InitialCreate
Appliquer la migration afin qu'un fichier sqLite soit créé

dotnet ef database update

🧪 Test API

👉 Swagger disponible sur : https://localhost:5001/swagger
🖥️ Front-end

    Partie Admin : Razor Pages

    Partie Membre : React.js (consomme l’API)

📅 Présentation

🗓 Date : 01 Août 2025
⏱ Durée : 15 minutes
📂 Livrables

Projet complet avec code source

Base de données PostgreSQL

Documentation API (README + Swagger)

    Interfaces front-end (bonus)

🎁 Bonus

    Système de rôles finement défini 🛡

    Réinitialisation mot de passe avec envoi mail 📧

    UI simple et fonctionnelle pour les membres et les admins 🧼

🙌 Auteurs

    👨‍💻 Développeurs backend : Maxime, Vincent, Othman et Cédric

    🏢 Client : BibliothèqueSimplon

En cours: Création espace utilisateur, gestion Reservation, gestion emprunt, connexion et register
