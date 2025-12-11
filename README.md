# Kashi – Smart Budget Management System (ASP.NET Core .NET 9) 💰📊

كاشي؟ أكيد كل واحد فينا مرّ عليه يوم وسأل نفسه:  
"هو أنا صرفت فلوسي في إيه؟"  
وبنفضل نقول إن الحسابات غلط… لكن الحقيقة إن طريقة الصرف نفسها هي السبب.

Kashi is a smart budgeting backend system designed to help users track expenses, understand spending patterns, and generate meaningful financial insights.  
Built using **ASP.NET Core (.NET 9)** and structured with **Clean Architecture**, the project is scalable, maintainable, and ready for real-world scenarios.

---

## 📌 Table of Contents
- Overview  
- Features  
- Clean Architecture Structure  
- API Modules  
- Screenshots  
- Tech Stack  
- How to Run  
- Future Enhancements  
- Repository Structure  
- Author  

---

## 📖 Overview
Kashi provides a structured and organized way to record daily transactions and automatically analyze spending behavior.  
It replaces manual tracking with clear summaries, categorized transactions, and AI-powered financial forecasting.

The system is designed with strict separation of concerns using Clean Architecture principles to ensure maintainability and scalability.

---

## ⭐ Features
- RESTful API with clean and consistent endpoint structure  
- Multi-currency account management  
- Categorized income & expense tracking  
- Monthly financial summaries  
- AI-powered forecasting 🤖  
- Budget planning and monitoring  
- Analytics endpoints for dashboards  
- SQL Server relational schema  
- Swagger API documentation 📑  
- Identity-based authentication 🔐  

---

## 🏗 Clean Architecture Structure
Kashi.Domain
Kashi.Application
Kashi.Infrastructure
Kashi.Api (Presentation Layer)

yaml
Copy code

---

## 🔌 API Modules (Summary)

### 🧾 Accounts
- Create / update / delete accounts  
- Retrieve balance  
- List all accounts  

### 💸 Transactions
- Add income / expense  
- Filter by date or category  
- Generate monthly summaries  

### 🗂 Categories
- CRUD operations for spending categories  

### 🤖 AI
- Monthly spending forecasting endpoint  

### 📉 Budget
- Define monthly budget and compare with actual spending  

### 📊 Analytics
- Structured data for dashboards  

---

## 📷 Screenshots

![img](https://i.ibb.co/7JYsCTGr/2.png)  
![img](https://i.ibb.co/wXrBdkV/3.png)  
![img](https://i.ibb.co/gZmpgB2K/4.png)  
![img](https://i.ibb.co/bgcwCtck/5.png)  
![img](https://i.ibb.co/vvfTfX7t/6.png)  
![img](https://i.ibb.co/B25z1cYq/7.png)  
![img](https://i.ibb.co/zV1X8CWn/Screenshot-2025-12-11-115606.png)

---

## 🛠 Tech Stack
- .NET 9  
- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- ASP.NET Identity  
- AutoMapper  
- Swagger / OpenAPI  
- LINQ  
- Postman  
- Docker (planned) 🐳  
- Git & GitHub  

---

## ▶ How to Run

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/mohamedfaresss/Kashi-SmartBudget
cd Kashi-SmartBudget
2️⃣ Configure Database
Update appsettings.json with your SQL Server connection string.

3️⃣ Apply Migrations
bash
Copy code
dotnet ef database update
4️⃣ Run the API
bash
Copy code
dotnet run
Swagger UI will be available at:

bash
Copy code
https://localhost:xxxx/swagger
🚀 Future Enhancements
Advanced AI insights

Improved budget planning tools

Rich data analytics dashboards

Frontend integration (web and mobile)

Full Docker support for deployment

📂 Repository Structure
mathematica
Copy code
/Kashi.Api
/Kashi.Application
/Kashi.Domain
/Kashi.Infrastructure
README.md
👤 Author
Developed by Mohamed Gamal Fares
Backend Developer (.NET)

GitHub: https://github.com/mohamedfaresss
LinkedIn: https://www.linkedin.com/in/mohamed-gamal-fares/

