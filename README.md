# Doctor’s Practice Management System

## 📌 Overview

This project was developed as part of a major university assignment. It is a **management system and web application** designed for a doctor’s practice, with integrated modules for:

* **Patient and appointment management**
* **Pharmacy operations** (inventory and prescriptions)
* **E-commerce store** for online orders

The application was built using the **.NET Framework (MVC pattern)** with **Entity Framework** for database management in **Visual Studio**.

---

## 🚀 Features

* 📅 **Appointment scheduling and patient records**
* 🏥 **Doctor’s dashboard** with medical history access
* 💊 **Pharmacy management** for stock and prescriptions
* 🛒 **E-commerce functionality** for online medicine ordering
* 🔐 **Role-based authentication** (doctor, pharmacist, patient, admin)

---

## 🛠️ Tech Stack

* **Framework:** .NET Framework (MVC)
* **Language:** C#
* **Database:** Entity Framework with SQL Server
* **IDE:** Visual Studio
* **UI:** Razor Views (ASP.NET MVC) + Bootstrap

---

## 📂 Project Structure

```
project-root/
│── Controllers/       # MVC Controllers
│── Models/            # Entity Framework Models
│── Views/             # Razor Views (UI layer)
│── Scripts/           # Frontend scripts (JS)
│── Content/           # CSS, images, assets
│── Database/          # EF migrations & schema
│── README.md          # Project overview
```

---

## 🔧 Installation & Setup

1. Clone the repository

   ```bash
   git clone https://github.com/yourusername/DoctorsApp-with-Pharmacy.git
   ```
2. Open the solution in **Visual Studio**.
3. Restore NuGet packages.
4. Update the **connection string** in `Web.config` to point to your SQL Server instance.
5. Run **Entity Framework migrations** to create the database.
6. Press **F5** in Visual Studio to build and run the application.

---

## 🎯 Usage

* **Doctors** → Manage patients, view medical records, schedule appointments.
* **Pharmacists** → Manage prescriptions and inventory.
* **Patients** → Book appointments and order medicines online.
* **Admins** → Manage system users and settings.

---

## 📖 Future Improvements

* Integrate online payment gateways
* Add support for telemedicine consultations
* Implement reporting and analytics dashboards

---

## 👩‍💻 Author

*Developed by \Cameron Peters* as part of a major university project using the **.NET MVC Framework with Entity Framework in Visual Studio**.
