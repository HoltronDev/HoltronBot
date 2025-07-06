CREATE TABLE stockpile (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    foodName TEXT NOT NULL,
    count INTEGER NOT NULL DEFAULT 0
);

INSERT INTO stockpile (foodName) VALUES ('pizzaSlice'), ('soda');