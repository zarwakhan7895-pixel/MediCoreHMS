-- ====================================================================
-- MEDICORE CITY HOSPITAL DATABASE
-- Group Members - Registration IDs: 65916, 67429, 63930
-- GCN = (27+28+21) % 5 = 76 % 5 = 1
-- GCN 1 => Dashboard shows Doctors On Duty + Admissions in Last 24 Hours
-- ====================================================================


-- --------------------------------------------------------------------
-- 1. CREATE DATABASE
-- --------------------------------------------------------------------
CREATE DATABASE MediCoreHMS;
GO

USE MediCoreHMS;
GO


-- --------------------------------------------------------------------
-- 2. CREATE TABLES
-- --------------------------------------------------------------------

CREATE TABLE Patients
(
    PatientId       INT IDENTITY(1,1) PRIMARY KEY,
    FullName        VARCHAR(150) NOT NULL,
    Age             INT NOT NULL,
    Gender          VARCHAR(20),
    ContactNumber   VARCHAR(30),
    Address         VARCHAR(250),
    MedicalHistory  VARCHAR(MAX),
    RegisteredOn    DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE Doctors
(
    DoctorId               INT IDENTITY(1,1) PRIMARY KEY,
    FullName               VARCHAR(150) NOT NULL,
    Specialization         VARCHAR(150),
    Ward                   VARCHAR(50),
    IsAvailable            BIT DEFAULT 1,
    ConsultationSchedule   VARCHAR(100)
);
GO

CREATE TABLE Beds
(
    BedId       INT IDENTITY(1,1) PRIMARY KEY,
    Ward        VARCHAR(50) NOT NULL,
    BedNumber   INT NOT NULL,
    IsOccupied  BIT DEFAULT 0
);
GO

CREATE TABLE Admissions
(
    AdmissionId         INT IDENTITY(1,1) PRIMARY KEY,
    PatientId           INT NOT NULL,
    DoctorId            INT NOT NULL,
    BedId               INT NOT NULL,
    Ward                VARCHAR(50),
    EntryType           VARCHAR(30),
    AdmissionDate       DATETIME DEFAULT GETDATE(),
    DischargeDate       DATETIME,
    TreatmentSummary    VARCHAR(MAX),
    IsDischarged        BIT DEFAULT 0,

    FOREIGN KEY (PatientId) REFERENCES Patients (PatientId),
    FOREIGN KEY (DoctorId)  REFERENCES Doctors (DoctorId),
    FOREIGN KEY (BedId)     REFERENCES Beds (BedId)
);
GO


-- --------------------------------------------------------------------
-- 3. SEED DOCTORS  (one per ward)
--    Schedule start time "07 AM" comes from first two digits of 67429
-- --------------------------------------------------------------------
INSERT INTO Doctors (FullName, Specialization, Ward, IsAvailable, ConsultationSchedule)
VALUES
('Dr. Ahmed Khan',    'General Physician', 'General',   1, '07:00 AM - 02:00 PM'),
('Dr. Sara Malik',    'Surgeon',           'ICU',       1, '07:00 AM - 02:00 PM'),
('Dr. Ayesha Raza',   'Gynecologist',      'Maternity', 1, '07:00 AM - 02:00 PM'),
('Dr. Bilal Hussain', 'Pediatrician',      'Pediatric', 1, '07:00 AM - 02:00 PM');
GO


-- --------------------------------------------------------------------
-- 4. SEED BEDS
--    Capacities scaled using middle digits "42" of registration 67429
--    General = 20, ICU = 10, Maternity = 8, Pediatric = 12
-- --------------------------------------------------------------------

-- General ward beds
INSERT INTO Beds (Ward, BedNumber) VALUES
('General', 1), ('General', 2),  ('General', 3),  ('General', 4),
('General', 5), ('General', 6),  ('General', 7),  ('General', 8),
('General', 9), ('General', 10), ('General', 11), ('General', 12),
('General', 13),('General', 14), ('General', 15), ('General', 16),
('General', 17),('General', 18), ('General', 19), ('General', 20);

-- ICU ward beds
INSERT INTO Beds (Ward, BedNumber) VALUES
('ICU', 1), ('ICU', 2), ('ICU', 3), ('ICU', 4), ('ICU', 5),
('ICU', 6), ('ICU', 7), ('ICU', 8), ('ICU', 9), ('ICU', 10);

-- Maternity ward beds
INSERT INTO Beds (Ward, BedNumber) VALUES
('Maternity', 1), ('Maternity', 2), ('Maternity', 3), ('Maternity', 4),
('Maternity', 5), ('Maternity', 6), ('Maternity', 7), ('Maternity', 8);

-- Pediatric ward beds
INSERT INTO Beds (Ward, BedNumber) VALUES
('Pediatric', 1), ('Pediatric', 2), ('Pediatric', 3),  ('Pediatric', 4),
('Pediatric', 5), ('Pediatric', 6), ('Pediatric', 7),  ('Pediatric', 8),
('Pediatric', 9), ('Pediatric', 10),('Pediatric', 11), ('Pediatric', 12);
GO


-- --------------------------------------------------------------------
-- 5. SEED PATIENTS
--    16 patients seeded because last two digits of 65916 = 16
-- --------------------------------------------------------------------
INSERT INTO Patients (FullName, Age, Gender, ContactNumber, Address, MedicalHistory, RegisteredOn)
VALUES
('Patient 1',  25, 'Female', '03000000001', 'Karachi', 'No prior history recorded', DATEADD(DAY, -1,  GETDATE())),
('Patient 2',  34, 'Male',   '03000000002', 'Karachi', 'No prior history recorded', DATEADD(DAY, -2,  GETDATE())),
('Patient 3',  41, 'Female', '03000000003', 'Karachi', 'No prior history recorded', DATEADD(DAY, -3,  GETDATE())),
('Patient 4',  19, 'Male',   '03000000004', 'Karachi', 'No prior history recorded', DATEADD(DAY, -4,  GETDATE())),
('Patient 5',  56, 'Female', '03000000005', 'Karachi', 'No prior history recorded', DATEADD(DAY, -5,  GETDATE())),
('Patient 6',  29, 'Male',   '03000000006', 'Karachi', 'No prior history recorded', DATEADD(DAY, -6,  GETDATE())),
('Patient 7',  47, 'Female', '03000000007', 'Karachi', 'No prior history recorded', DATEADD(DAY, -7,  GETDATE())),
('Patient 8',  33, 'Male',   '03000000008', 'Karachi', 'No prior history recorded', DATEADD(DAY, -8,  GETDATE())),
('Patient 9',  60, 'Female', '03000000009', 'Karachi', 'No prior history recorded', DATEADD(DAY, -9,  GETDATE())),
('Patient 10', 22, 'Male',   '03000000010', 'Karachi', 'No prior history recorded', DATEADD(DAY, -10, GETDATE())),
('Patient 11', 38, 'Female', '03000000011', 'Karachi', 'No prior history recorded', DATEADD(DAY, -11, GETDATE())),
('Patient 12', 45, 'Male',   '03000000012', 'Karachi', 'No prior history recorded', DATEADD(DAY, -12, GETDATE())),
('Patient 13', 30, 'Female', '03000000013', 'Karachi', 'No prior history recorded', DATEADD(DAY, -13, GETDATE())),
('Patient 14', 52, 'Male',   '03000000014', 'Karachi', 'No prior history recorded', DATEADD(DAY, -14, GETDATE())),
('Patient 15', 27, 'Female', '03000000015', 'Karachi', 'No prior history recorded', DATEADD(DAY, -15, GETDATE())),
('Patient 16', 36, 'Male',   '03000000016', 'Karachi', 'No prior history recorded', DATEADD(DAY, -16, GETDATE()));
GO


-- --------------------------------------------------------------------
-- 6. SEED SAMPLE ADMISSIONS  (kept within last 24 hours so the
--    dashboard's "Admissions in Last 24 Hours" widget has data)
-- --------------------------------------------------------------------
INSERT INTO Admissions (PatientId, DoctorId, BedId, Ward, EntryType, AdmissionDate, IsDischarged)
VALUES
(1, 1, 1,  'General', 'OPD',       DATEADD(HOUR, -3, GETDATE()), 0),
(2, 2, 21, 'ICU',     'Emergency', DATEADD(HOUR, -1, GETDATE()), 0);
GO

UPDATE Beds SET IsOccupied = 1 WHERE BedId IN (1, 21);
GO


-- --------------------------------------------------------------------
-- 7. QUICK VERIFICATION QUERIES
-- --------------------------------------------------------------------
SELECT * FROM Patients;
SELECT * FROM Doctors;
SELECT * FROM Beds;
SELECT * FROM Admissions;
