-- Insertion des employés
INSERT INTO Employes (Nom, Prenom, Email, MotDePasse, Role) VALUES
                                                                ('Durand', 'Claire', 'claire.durand@biblio.fr', 'Password1', 'Administrateur'),
                                                                ('Martin', 'Jean', 'jean.martin@biblio.fr', 'Biblio123', 'Bibliothecaire');

-- Insertion des membres
INSERT INTO Membres (Nom, Prenom, Email, MotDePasse) VALUES
                                                         ('Lemoine', 'Sophie', 'sophie.lemoine@gmail.com', 'Membre123'),
                                                         ('Bernard', 'Luc', 'luc.bernard@gmail.com', 'LucMembre1');

-- Insertion des livres
INSERT INTO Livres (Categorie, Etat, Titre, Auteur, Description, AnneePublication, Editeur) VALUES
                                                                                                ('Roman', 'Disponible', '1984', 'George Orwell', 'Roman dystopique', 1949, 'Secker & Warburg'),
                                                                                                ('Conte', 'Emprunte', 'Le Petit Prince', 'Antoine de Saint-Exupéry', 'Conte poétique et philosophique', 1943, 'Gallimard');

-- Insertion d'emprunts
INSERT INTO Emprunts (DateEmprunt, DateRetour, EmployeId, MembreId) VALUES
                                                                        ('2025-07-01', '2025-07-15', 1, 1),
                                                                        ('2025-07-05', '2025-07-20', 2, 2);

-- Insertion dans EmpruntLivres
INSERT INTO EmpruntLivres (IdLivre, IdEmprunt) VALUES
                                                   (1, 1),
                                                   (2, 2);

-- Insertion de réservations
INSERT INTO Reservations (LivreId, MembreId, DateReservation, EstActive) VALUES
                                                                             (1, 2, '2025-07-10', 1),
                                                                             (2, 1, '2025-07-12', 1);
