-- Skapa tabellen ClassLevels f�r att lagra niv�er
CREATE TABLE ClassLevels (
    Level_id INT IDENTITY(1,1) PRIMARY KEY,
    LevelName VARCHAR(50) NOT NULL UNIQUE
);

-- L�gg till niv�er till ClassLevels
INSERT INTO ClassLevels (LevelName)
VALUES ('beginner'), ('intermediate'), ('advanced');

-- Skapa tabellen Classes f�r att lagra dansstilar och koppla till niv�er
CREATE TABLE Classes (
    class_id INT IDENTITY(1,1) PRIMARY KEY,
    class_name VARCHAR(50) NOT NULL, -- Endast stilnamnet, t.ex. 'ballet', 'salsa'
    Level_id INT NOT NULL,           -- Koppling till ClassLevels
    FOREIGN KEY (Level_id) REFERENCES ClassLevels(Level_id)
);

-- Skapa tabellen DanceStyle (kan anv�ndas f�r framtida stilinformation)
CREATE TABLE DanceStyle (
    Style_id INT IDENTITY(1,1) PRIMARY KEY,
    StyleName VARCHAR(25) NOT NULL UNIQUE
);

-- Skapa tabellen Student f�r att lagra studentinformation
CREATE TABLE Student (
    Student_id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(30) NOT NULL,
    LastName VARCHAR(30) NOT NULL,
    DateOfBirth DATE NOT NULL
);

-- Skapa tabellen Instructors f�r att lagra instrukt�rsinformation
CREATE TABLE Instructors (
    instructor_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    style VARCHAR(50) NOT NULL, -- Kan kopplas till DanceStyle om behov finns
    CONSTRAINT chk_style CHECK (style IN ('ballet', 'salsa', 'bachata', 'hiphop'))
);

-- Skapa tabellen Class_schedule f�r att lagra schemainformation
CREATE TABLE Class_schedule (
    schedule_id INT IDENTITY(1,1) PRIMARY KEY,
    class_id INT NOT NULL,
    instructor_id INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    FOREIGN KEY (instructor_id) REFERENCES Instructors(instructor_id)
);

-- Skapa tabellen Bookings f�r att hantera bokningar
CREATE TABLE Bookings (
    booking_id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    schedule_id INT NOT NULL,
    booking_date DATE NOT NULL,
    status VARCHAR(20) NOT NULL CHECK (status IN ('Confirmed', 'Cancelled')),
    FOREIGN KEY (student_id) REFERENCES Student(Student_id),
    FOREIGN KEY (schedule_id) REFERENCES Class_schedule(schedule_id)
);
