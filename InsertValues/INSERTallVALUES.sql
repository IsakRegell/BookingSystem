-- Lägg till dansklasser i Classes
INSERT INTO Classes (class_name, Level_id)
VALUES 
    ('ballet', 1),    -- beginner
    ('salsa', 2),     -- intermediate
    ('hiphop', 3),    -- advanced
    ('bachata', 2);   -- intermediate

-- Lägg till dansstilar i DanceStyle
INSERT INTO DanceStyle (StyleName)
VALUES 
    ('ballet'), 
    ('salsa'), 
    ('hiphop'), 
    ('bachata');

-- Lägg till studenter i Student
INSERT INTO Student (FirstName, LastName, DateOfBirth)
VALUES 
    ('Anna', 'Svensson', '2000-03-15'),
    ('Erik', 'Johansson', '1998-07-22'),
    ('Lisa', 'Karlsson', '2002-12-05'),
    ('David', 'Nilsson', '1995-05-10');

-- Lägg till instruktörer i Instructors
INSERT INTO Instructors (first_name, last_name, style)
VALUES 
    ('Maria', 'Andersson', 'ballet'),
    ('Johan', 'Berg', 'salsa'),
    ('Emma', 'Lind', 'hiphop'),
    ('Oscar', 'Holm', 'bachata');

-- Lägg till klasser i Class_schedule
INSERT INTO Class_schedule (class_id, instructor_id, start_date, end_date)
VALUES 
    (1, 1, '2025-01-10', '2025-03-10'),  -- ballet, Maria Andersson
    (2, 2, '2025-02-01', '2025-04-01'),  -- salsa, Johan Berg
    (3, 3, '2025-03-05', '2025-05-05'),  -- hiphop, Emma Lind
    (4, 4, '2025-04-10', '2025-06-10');  -- bachata, Oscar Holm

-- Lägg till bokningar i Bookings
INSERT INTO Bookings (student_id, schedule_id, booking_date, status)
VALUES 
    (1, 1, '2025-01-05', 'Confirmed'),   -- Anna bokar ballet
    (2, 2, '2025-01-15', 'Confirmed'),   -- Erik bokar salsa
    (3, 3, '2025-02-10', 'Cancelled'),   -- Lisa bokar hiphop
    (4, 4, '2025-03-20', 'Confirmed');   -- David bokar bachata