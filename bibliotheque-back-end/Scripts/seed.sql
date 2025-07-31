-- Membres
INSERT INTO "Membres" ("Nom", "Prenom", "Email", "MotDePasse") VALUES
                                                                   ('Dupont', 'Alice', 'alice.dupont@example.com', 'hashed_password_1'),
                                                                   ('Martin', 'Bernard', 'bernard.martin@example.com', 'hashed_password_2'),
                                                                   ('Petit', 'Claire', 'claire.petit@example.com', 'hashed_password_3'),
                                                                   ('Durand', 'David', 'david.durand@example.com', 'hashed_password_4'),
                                                                   ('Leroy', 'Emilie', 'emilie.leroy@example.com', 'hashed_password_5'),
                                                                   ('Moreau', 'Francois', 'francois.moreau@example.com', 'hashed_password_6'),
                                                                   ('Fournier', 'Sophie', 'sophie.fournier@example.com', 'hashed_password_7'),
                                                                   ('Girard', 'Thomas', 'thomas.girard@example.com', 'hashed_password_8'),
                                                                   ('Bonnet', 'Laura', 'laura.bonnet@example.com', 'hashed_password_9'),
                                                                   ('Roux', 'Nicolas', 'nicolas.roux@example.com', 'hashed_password_10'),
                                                                   ('Dubois', 'Jeanne', 'jeanne.dubois@example.com', 'hashed_password_11'),
                                                                   ('Lambert', 'Marc', 'marc.lambert@example.com', 'hashed_password_12'),
                                                                   ('Garcia', 'Chloé', 'chloe.garcia@example.com', 'hashed_password_13'),
                                                                   ('Richard', 'Antoine', 'antoine.richard@example.com', 'hashed_password_14'),
                                                                   ('Blanc', 'Manon', 'manon.blanc@example.com', 'hashed_password_15'),
                                                                   ('Simon', 'Lucas', 'lucas.simon@example.com', 'hashed_password_16'),
                                                                   ('Faure', 'Camille', 'camille.faure@example.com', 'hashed_password_17'),
                                                                   ('Giraud', 'Hugo', 'hugo.giraud@example.com', 'hashed_password_18'),
                                                                   ('Perrin', 'Juliette', 'juliette.perrin@example.com', 'hashed_password_19'),
                                                                   ('Sanchez', 'Gabriel', 'gabriel.sanchez@example.com', 'hashed_password_20');

-- Employes
-- Correction : Utilisation des valeurs numériques pour le rôle (0 pour Administrateur, 1 pour Bibliothecaire)
INSERT INTO "Employes" ("Nom", "Prenom", "Email", "MotDePasse", "Role") VALUES
                                                                            ('Dubois', 'Jean', 'jean.dubois@biblio.com', 'hashed_admin_password_1', 0), -- Administrateur
                                                                            ('Lefevre', 'Marie', 'marie.lefevre@biblio.com', 'hashed_bib_password_1', 1), -- Bibliothecaire
                                                                            ('Michel', 'Paul', 'paul.michel@biblio.com', 'hashed_bib_password_2', 1); -- Bibliothecaire

-- Livres
-- Correction : Utilisation des valeurs numériques pour la catégorie et l'état
INSERT INTO "Livres" ("Titre", "Auteur", "Description", "AnneePublication", "Editeur", "Categorie", "Etat") VALUES
                                                                                                                ('Les Misérables', 'Victor Hugo', 'Un classique de la littérature française.', 1862, 'Folio', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Dune', 'Frank Herbert', 'Un chef-d''œuvre de science-fiction.', 1965, 'Pocket', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Seigneur des Anneaux', 'J.R.R. Tolkien', 'Une épopée fantastique.', 1954, 'Gallimard', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Faucon Maltais', 'Dashiell Hammett', 'Un roman noir classique.', 1930, 'Gallimard', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Sapiens: Une brève histoire de l''humanité', 'Yuval Noah Harari', 'Une exploration fascinante de l''histoire humaine.', 2011, 'Albin Michel', 4, 0), -- Histoire (4), Disponible (0)
                                                                                                                ('L''Étranger', 'Albert Camus', 'Un roman philosophique existentaliste.', 1942, 'Gallimard', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Astérix le Gaulois', 'René Goscinny', 'La première aventure de l''irréductible Gaulois.', 1961, 'Hachette', 8, 0), -- BandeDessinee (8), Disponible (0)
                                                                                                                ('Code Complete', 'Steve McConnell', 'Un guide essentiel pour le développement logiciel.', 1993, 'Microsoft Press', 9, 0), -- Informatique (9), Disponible (0)
                                                                                                                ('L''Art de la Guerre', 'Sun Tzu', 'Un traité militaire stratégique.', -500, 'Flammarion', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Le Petit Prince', 'Antoine de Saint-Exupéry', 'Une fable poétique et philosophique.', 1943, 'Gallimard', 7, 0), -- Jeunesse (7), Disponible (0)
                                                                                                                ('1984', 'George Orwell', 'Un roman dystopique classique.', 1949, 'Gallimard', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Cent Ans de Solitude', 'Gabriel García Márquez', 'Un roman emblématique du réalisme magique.', 1967, 'Seuil', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Nom de la Rose', 'Umberto Eco', 'Un thriller médiéval intellectuel.', 1980, 'Grasset', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Fahrenheit 451', 'Ray Bradbury', 'Une dystopie sur la censure et la destruction des livres.', 1953, 'Denoël', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Fondation', 'Isaac Asimov', 'Le premier volume d''une saga de science-fiction majeure.', 1951, 'Folio SF', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Orgueil et Préjugés', 'Jane Austen', 'Un classique de la littérature romantique anglaise.', 1813, 'Le Livre de Poche', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Don Quichotte', 'Miguel de Cervantes', 'Considéré comme le premier roman moderne.', 1605, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Crime et Châtiment', 'Fiodor Dostoïevski', 'Un chef-d''œuvre de la littérature russe explorant la morale.', 1866, 'Actes Sud', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Les Aventures de Tintin : Le Lotus Bleu', 'Hergé', 'Une aventure emblématique de Tintin en Chine.', 1936, 'Casterman', 8, 0), -- BandeDessinee (8), Disponible (0)
                                                                                                                ('Cosmos', 'Carl Sagan', 'Une exploration de l''univers et de la science.', 1980, 'Points', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Histoire de France', 'Fernand Braudel', 'Une œuvre monumentale sur l''histoire de France.', 1986, 'Pluriel', 4, 0), -- Histoire (4), Disponible (0)
                                                                                                                ('Ainsi parlait Zarathoustra', 'Friedrich Nietzsche', 'Un ouvrage philosophique majeur.', 1883, 'Folio Essais', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Le Code Da Vinci', 'Dan Brown', 'Un thriller à succès mêlant art, histoire et complots.', 2003, 'Jean-Claude Lattès', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Guide du voyageur galactique', 'Douglas Adams', 'Une comédie de science-fiction culte.', 1979, 'Gallimard', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Vingt Mille Lieues sous les mers', 'Jules Verne', 'Un classique de l''aventure et de la science-fiction.', 1870, 'Hachette', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Harry Potter à l''école des sorciers', 'J.K. Rowling', 'Le début de la célèbre saga magique.', 1997, 'Gallimard Jeunesse', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Silmarillion', 'J.R.R. Tolkien', 'L''histoire mythologique de la Terre du Milieu.', 1977, 'Pocket', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Kafka sur le rivage', 'Haruki Murakami', 'Un roman onirique et mystérieux.', 2002, 'Belfond', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Une brève histoire du temps', 'Stephen Hawking', 'Un ouvrage de vulgarisation sur la cosmologie.', 1988, 'Flammarion', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('L''Alchimiste', 'Paulo Coelho', 'Un conte philosophique sur la quête de soi.', 1988, 'Anne Carrière', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Maus', 'Art Spiegelman', 'Une bande dessinée autobiographique sur la Shoah.', 1986, 'Flammarion', 8, 0), -- BandeDessinee (8), Disponible (0)
-- Nouveaux livres ajoutés
                                                                                                                ('Le Monde de Sophie', 'Jostein Gaarder', 'Un roman sur l''histoire de la philosophie.', 1991, 'Seuil', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Blade Runner', 'Philip K. Dick', 'Un classique de la science-fiction cyberpunk.', 1968, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Parfum', 'Patrick Süskind', 'L''histoire d''un tueur en série obsédé par les odeurs.', 1985, 'Fayard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Nom du Vent', 'Patrick Rothfuss', 'Le premier tome d''une série de fantasy épique.', 2007, 'Bragelonne', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('La Nuit des Temps', 'René Barjavel', 'Un roman de science-fiction post-apocalyptique.', 1968, 'Pocket', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Bruit et la Fureur', 'William Faulkner', 'Un roman moderniste emblématique.', 1929, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Les Fleurs du Mal', 'Charles Baudelaire', 'Un recueil de poèmes majeurs.', 1857, 'Folio', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Rouge et le Noir', 'Stendhal', 'Un roman réaliste du XIXe siècle.', 1830, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Voyage au bout de la nuit', 'Louis-Ferdinand Céline', 'Un roman picaresque et antimilitariste.', 1932, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('L''Insoutenable Légèreté de l''être', 'Milan Kundera', 'Un roman philosophique sur l''existence.', 1984, 'Gallimard', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Le Zéro et l''Infini', 'Arthur Koestler', 'Un roman politique sur les purges staliniennes.', 1940, 'Calmann-Lévy', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('La Route', 'Cormac McCarthy', 'Un roman post-apocalyptique sombre.', 2006, 'L''Olivier', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Cycle de Cyann', 'François Bourgeon', 'Une série de bande dessinée de science-fiction.', 1993, 'Casterman', 8, 0), -- BandeDessinee (8), Disponible (0)
                                                                                                                ('L''Homme qui plantait des arbres', 'Jean Giono', 'Un court récit écologique et philosophique.', 1953, 'Gallimard', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Le Horla', 'Guy de Maupassant', 'Une nouvelle fantastique sur la folie.', 1887, 'Folio', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Les Trois Mousquetaires', 'Alexandre Dumas', 'Un roman d''aventure historique.', 1844, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Vendredi ou la Vie sauvage', 'Michel Tournier', 'Une réécriture du mythe de Robinson Crusoé.', 1971, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Grand Meaulnes', 'Alain-Fournier', 'Un roman initiatique.', 1913, 'Le Livre de Poche', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('La Peste', 'Albert Camus', 'Un roman allégorique sur l''absurdité de l''existence.', 1947, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Petit Nicolas', 'René Goscinny', 'Les aventures d''un petit garçon espiègle.', 1959, 'Denöel', 7, 0), -- Jeunesse (7), Disponible (0)
                                                                                                                ('Les Fourmis', 'Bernard Werber', 'Un roman de science-fiction explorant le monde des fourmis.', 1991, 'Albin Michel', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('La Promesse de l''aube', 'Romain Gary', 'Une autobiographie romancée.', 1960, 'Gallimard', 5, 0), -- Biographie (5), Disponible (0)
                                                                                                                ('Le Transperceneige', 'Jacques Lob', 'Une bande dessinée post-apocalyptique.', 1982, 'Casterman', 8, 0), -- BandeDessinee (8), Disponible (0)
                                                                                                                ('L''Art de la simplicité', 'Dominique Loreau', 'Un guide sur le minimalisme et l''organisation.', 2005, 'Flammarion', 10, 0), -- Art (10), Disponible (0)
                                                                                                                ('Mange, Prie, Aime', 'Elizabeth Gilbert', 'Un récit de voyage et de quête personnelle.', 2006, 'Calmann-Lévy', 11, 0), -- Voyage (11), Disponible (0)
                                                                                                                ('La Cuisine de Référence', 'Michel Maincent-Morel', 'Un ouvrage fondamental pour la cuisine française.', 2002, 'BPI', 12, 0), -- Cuisine (12), Disponible (0)
                                                                                                                ('Le Guide des aliments qui soignent', 'Jean-Marie Delecroix', 'Un livre sur les bienfaits des aliments pour la santé.', 2010, 'Marabout', 13, 0), -- Sante (13), Disponible (0)
                                                                                                                ('Le Monde comme volonté et comme représentation', 'Arthur Schopenhauer', 'Un œuvre majeure de la philosophie allemande.', 1818, 'PUF', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Les Chroniques de Narnia : Le Lion, la Sorcière blanche et l''Armoire magique', 'C.S. Lewis', 'Le premier volume de la série fantastique.', 1950, 'Gallimard Jeunesse', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Hobbit', 'J.R.R. Tolkien', 'Le prélude au Seigneur des Anneaux.', 1937, 'Pocket', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Problème à trois corps', 'Liu Cixin', 'Un roman de science-fiction chinois primé.', 2008, 'Actes Sud', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('La Servante écarlate', 'Margaret Atwood', 'Un roman dystopique féministe.', 1985, 'Robert Laffont', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Chien des Baskerville', 'Arthur Conan Doyle', 'Une enquête de Sherlock Holmes.', 1902, 'Le Livre de Poche', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('L''Histoire sans fin', 'Michael Ende', 'Un roman fantastique pour la jeunesse.', 1979, 'Gallimard Jeunesse', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('La Cité des chimères', 'China Miéville', 'Un roman de fantasy urbaine.', 2009, 'Fleuve Noir', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Bâtard de Kosigan', 'Fabien Clavel', 'Un roman historique et fantastique.', 2011, 'Bragelonne', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('La Couleur tombée du ciel', 'H.P. Lovecraft', 'Une nouvelle d''horreur cosmique.', 1927, 'J''ai Lu', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Cycle de l''Héritage : Eragon', 'Christopher Paolini', 'Un roman de fantasy jeunesse.', 2002, 'Bayard Jeunesse', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Les Annales du Disque-monde : Le Huitième Sortilège', 'Terry Pratchett', 'Une parodie de fantasy.', 1983, 'L''Atalante', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Puits des Ténèbres', 'Serge Brussolo', 'Un roman fantastique et d''horreur.', 1986, 'Fleuve Noir', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Cycle de la Tour Sombre : Le Pistolero', 'Stephen King', 'Le premier volume d''une saga épique.', 1982, 'J''ai Lu', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('American Gods', 'Neil Gaiman', 'Un roman de fantasy urbaine et mythologique.', 2001, 'Au Diable Vauvert', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Labyrinthe de Pan', 'Guillermo del Toro', 'Un conte de fées sombre pour adultes.', 2006, 'Points', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('L''Appel de Cthulhu', 'H.P. Lovecraft', 'Une nouvelle d''horreur cosmique.', 1928, 'J''ai Lu', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('La Communauté de l''Anneau', 'J.R.R. Tolkien', 'Le premier tome du Seigneur des Anneaux.', 1954, 'Gallimard', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Trône de Fer : Le Donjon de glace', 'George R.R. Martin', 'Le premier volume de la saga.', 1996, 'J''ai Lu', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Sorceleur : Le Dernier Vœu', 'Andrzej Sapkowski', 'Le premier recueil de nouvelles.', 1993, 'Bragelonne', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Les Royaumes du Nord', 'Philip Pullman', 'Le premier tome de À la croisée des mondes.', 1995, 'Gallimard Jeunesse', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('Le Cycle de la Terre mourante', 'Jack Vance', 'Une série de fantasy et de science-fiction.', 1950, 'J''ai Lu', 2, 0), -- Fantastique (2), Disponible (0)
                                                                                                                ('La Geste des Princes-Démons', 'Jack Vance', 'Une série de science-fiction.', 1964, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Ubik', 'Philip K. Dick', 'Un roman de science-fiction psychédélique.', 1969, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Meilleur des mondes', 'Aldous Huxley', 'Un roman dystopique classique.', 1932, 'Pocket', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Un cantique pour Leibowitz', 'Walter M. Miller Jr.', 'Un roman post-apocalyptique.', 1959, 'Gallimard', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('La Guerre des mondes', 'H.G. Wells', 'Un classique de l''invasion extraterrestre.', 1898, 'Le Livre de Poche', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Temps machine', 'H.G. Wells', 'Un roman de voyage dans le temps.', 1895, 'Le Livre de Poche', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Les Enfants d''Icare', 'Arthur C. Clarke', 'Un roman de science-fiction sur l''évolution humaine.', 1953, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Rendez-vous avec Rama', 'Arthur C. Clarke', 'Un roman de science-fiction sur une rencontre extraterrestre.', 1973, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Solaris', 'Stanislaw Lem', 'Un roman de science-fiction philosophique.', 1961, 'Folio SF', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Maître du Haut Château', 'Philip K. Dick', 'Un roman de science-fiction uchronique.', 1962, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('La Planète des singes', 'Pierre Boulle', 'Un roman de science-fiction dystopique.', 1963, 'Pocket', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('La Main gauche de la nuit', 'Ursula K. Le Guin', 'Un roman de science-fiction féministe.', 1969, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Hypérion', 'Dan Simmons', 'Un roman de science-fiction épique.', 1989, 'Pocket', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Cycle de Fondation : Prélude à Fondation', 'Isaac Asimov', 'Un prélude à la saga Fondation.', 1988, 'Folio SF', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Neuromancien', 'William Gibson', 'Un roman cyberpunk fondateur.', 1984, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('La Stratégie Ender', 'Orson Scott Card', 'Un roman de science-fiction militaire.', 1985, 'J''ai Lu', 1, 0), -- ScienceFiction (1), Disponible (0)
                                                                                                                ('Le Mur invisible', 'Marlen Haushofer', 'Un roman dystopique autrichien.', 1963, 'Actes Sud', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('La Route de Coromandel', 'Jean-Christophe Rufin', 'Un roman d''aventure historique.', 1997, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Cercle littéraire des amateurs de tourte au pelure de patate', 'Mary Ann Shaffer', 'Un roman épistolaire pendant la Seconde Guerre mondiale.', 2008, 'Nil Éditions', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('L''Ombre du vent', 'Carlos Ruiz Zafón', 'Un roman gothique et mystérieux.', 2001, 'Grasset', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Liseur', 'Bernhard Schlink', 'Un roman sur l''amour, la culpabilité et l''Holocauste.', 1995, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('La Vie devant soi', 'Romain Gary', 'Un roman touchant sur l''amitié et la survie.', 1975, 'Gallimard', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Journal d''Anne Frank', 'Anne Frank', 'Le témoignage d''une jeune fille pendant l''Holocauste.', 1947, 'Le Livre de Poche', 5, 0), -- Biographie (5), Disponible (0)
                                                                                                                ('De sang-froid', 'Truman Capote', 'Un roman non-fictionnel sur un meurtre.', 1966, 'Gallimard', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Dahlia noir', 'James Ellroy', 'Un roman noir basé sur un fait divers réel.', 1987, 'Rivages/Noir', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Millénium : Les Hommes qui n''aimaient pas les femmes', 'Stieg Larsson', 'Le premier tome de la trilogie policière.', 2005, 'Actes Sud', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Silence des agneaux', 'Thomas Harris', 'Un thriller psychologique.', 1988, 'Pocket', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Gone Girl', 'Gillian Flynn', 'Un thriller psychologique.', 2012, 'Sonatine Éditions', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Les Dix Petits Nègres', 'Agatha Christie', 'Un classique du roman policier.', 1939, 'Le Livre de Poche', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Mystère de la Chambre Jaune', 'Gaston Leroux', 'Un roman policier à énigme.', 1907, 'Le Livre de Poche', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Chien jaune', 'Georges Simenon', 'Une enquête du commissaire Maigret.', 1931, 'Le Livre de Poche', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Grand Sommeil', 'Raymond Chandler', 'Un roman noir classique.', 1939, 'Gallimard', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('L.A. Confidential', 'James Ellroy', 'Un roman noir complexe.', 1990, 'Rivages/Noir', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Nom de la Bête', 'Philippe Claudel', 'Un roman policier et philosophique.', 2012, 'Stock', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Serment des limbes', 'Jean-Christophe Grangé', 'Un thriller haletant.', 2007, 'Albin Michel', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Les Rivières pourpres', 'Jean-Christophe Grangé', 'Un thriller sombre et violent.', 1998, 'Albin Michel', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('La Fille du train', 'Paula Hawkins', 'Un thriller psychologique à succès.', 2015, 'Sonatine Éditions', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Patient', 'Bernard Minier', 'Un thriller policier français.', 2019, 'XO Éditions', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('La Vérité sur l''affaire Harry Quebert', 'Joël Dicker', 'Un thriller littéraire.', 2012, 'Éditions de Fallois', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('Le Tueur en série', 'Maxime Chattam', 'Un thriller horrifique.', 2000, 'Michel Lafon', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('L''Appel du coucou', 'Robert Galbraith (J.K. Rowling)', 'Un roman policier.', 2013, 'Grasset', 3, 0), -- Policier (3), Disponible (0)
                                                                                                                ('La Chambre des merveilles', 'Julien Sandrel', 'Un roman feel-good.', 2018, 'Calmann-Lévy', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Changer l''eau des fleurs', 'Valérie Perrin', 'Un roman sur la vie et la mort.', 2018, 'Albin Michel', 0, 0), -- Roman (0), Disponible (0)
                                                                                                                ('Le Crépuscule et l''Aube', 'Ken Follett', 'Un roman historique.', 2020, 'Robert Laffont', 4, 0), -- Histoire (4), Disponible (0)
                                                                                                                ('La Bibliothèque de Minuit', 'Matt Haig', 'Un roman feel-good et philosophique.', 2020, 'Mazarine', 6, 0), -- Philosophie (6), Disponible (0)
                                                                                                                ('Le Fabuleux Destin d''Amélie Poulain', 'Guillaume Laurant', 'Le scénario du film culte.', 2001, 'Points', 0, 0); -- Roman (0), Disponible (0)

-- Emprunts (assuming MembreId and LivreId correspond to existing records)
INSERT INTO "Emprunts" ("MembreId", "DateEmprunt", "DateRetour") VALUES
                                                                     (1, '2024-07-01', '2024-07-15'),
                                                                     (2, '2024-07-05', NULL), -- Currently borrowed
                                                                     (1, '2024-07-10', '2024-07-20'),
                                                                     (3, '2024-07-12', NULL),
                                                                     (4, '2024-07-15', '2024-07-25'),
                                                                     (5, '2024-07-18', NULL),
                                                                     (6, '2024-07-20', '2024-07-28'),
                                                                     (7, '2024-07-22', NULL),
                                                                     (8, '2024-07-25', '2024-07-30'),
                                                                     (9, '2024-07-28', NULL);

-- EmpruntLivres (linking emprunts to specific books)
-- Correction : Utilisation des noms de colonnes corrects ("empruntId" et "livreId")
-- Ensure Livre IDs and Emprunt IDs exist before inserting these.
-- Adjust Livre IDs based on the actual IDs generated by your database.
INSERT INTO "EmpruntLivres" ("empruntId", "livreId") VALUES
                                                         (1, 1),  -- Membre 1 borrowed Les Misérables
                                                         (2, 2),  -- Membre 2 borrowed Dune
                                                         (3, 3),  -- Membre 1 borrowed Le Seigneur des Anneaux
                                                         (4, 4),  -- Membre 3 borrowed Le Faucon Maltais
                                                         (5, 5),  -- Membre 4 borrowed Sapiens
                                                         (6, 6),  -- Membre 5 borrowed L'Étranger
                                                         (7, 7),  -- Membre 6 borrowed Astérix le Gaulois
                                                         (8, 8),  -- Membre 7 borrowed Code Complete
                                                         (9, 9),  -- Membre 8 borrowed L'Art de la Guerre
                                                         (10, 10), -- Membre 9 borrowed Le Petit Prince
                                                         (1, 11), -- Membre 1 borrowed 1984 (example of multiple books per emprunt)
                                                         (2, 12), -- Membre 2 borrowed Cent Ans de Solitude
                                                         (3, 13), -- Membre 1 borrowed Le Nom de la Rose
                                                         (4, 14), -- Membre 3 borrowed Fahrenheit 451
                                                         (5, 15), -- Membre 4 borrowed Fondation
                                                         (6, 16), -- Membre 5 borrowed Orgueil et Préjugés
                                                         (7, 17), -- Membre 6 borrowed Don Quichotte
                                                         (8, 18), -- Membre 7 borrowed Crime et Châtiment
                                                         (9, 19), -- Membre 8 borrowed Les Aventures de Tintin
                                                         (10, 20), -- Membre 9 borrowed Cosmos
                                                         (1, 21), -- Membre 1 borrowed Histoire de France
                                                         (2, 22), -- Membre 2 borrowed Ainsi parlait Zarathoustra
                                                         (3, 23), -- Membre 1 borrowed Le Code Da Vinci
                                                         (4, 24), -- Membre 3 borrowed Le Guide du voyageur galactique
                                                         (5, 25), -- Membre 4 borrowed Vingt Mille Lieues sous les mers
                                                         (6, 26), -- Membre 5 borrowed Harry Potter à l'école des sorciers
                                                         (7, 27), -- Membre 6 borrowed Le Silmarillion
                                                         (8, 28), -- Membre 7 borrowed Kafka sur le rivage
                                                         (9, 29), -- Membre 8 borrowed Une brève histoire du temps
                                                         (10, 30); -- Membre 9 borrowed L'Alchimiste


-- Reservations
-- Correction : Utilisation de TRUE/FALSE pour la colonne "EstActive"
INSERT INTO "Reservations" ("LivreId", "MembreId", "DateReservation", "EstActive") VALUES
                                                                                       (21, 1, '2024-07-25', TRUE), -- Membre 1 reserved Histoire de France
                                                                                       (22, 2, '2024-07-26', TRUE), -- Membre 2 reserved Ainsi parlait Zarathoustra
                                                                                       (23, 3, '2024-07-27', TRUE), -- Membre 3 reserved Le Code Da Vinci
                                                                                       (24, 4, '2024-07-28', FALSE), -- Membre 4 reserved Le Guide du voyageur galactique (inactive)
                                                                                       (25, 5, '2024-07-29', TRUE), -- Membre 5 reserved Vingt Mille Lieues sous les mers
                                                                                       (26, 6, '2024-07-30', TRUE), -- Membre 6 reserved Harry Potter
                                                                                       (27, 7, '2024-07-31', FALSE), -- Membre 7 reserved Le Silmarillion (inactive)
                                                                                       (28, 8, '2024-08-01', TRUE), -- Membre 8 reserved Kafka sur le rivage
                                                                                       (29, 9, '2024-08-02', TRUE), -- Membre 9 reserved Une brève histoire du temps
                                                                                       (30, 10, '2024-08-03', TRUE); -- Membre 10 reserved L'Alchimiste
