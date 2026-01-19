# Job Application Tracker

A full-stack job application tracking system that helps you organize and manage your job search process. Track applications, monitor their status, and keep all your job hunting information in one place.

## 🚀 Features

- **Application Management**: Create, update, and delete job applications
- **Status Tracking**: Monitor application progress (Applied, Interviewing, Offered, Rejected)
- **Document Management**: Track resumes and cover letters for each application
- **User Authentication**: Secure JWT-based authentication with ASP.NET Identity
- **Responsive Design**: Modern, mobile-friendly interface built with Next.js and Tailwind CSS
- **RESTful API**: Well-structured ASP.NET Core Web API

## 🛠️ Tech Stack

### Frontend

- **Next.js 16** - React framework with App Router
- **React 19** - UI library
- **TypeScript** - Type-safe JavaScript
- **Tailwind CSS 4** - Utility-first CSS framework
- **Axios** - HTTP client
- **Jest & React Testing Library** - Testing framework

### Backend

- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for database access
- **ASP.NET Identity** - User authentication and authorization
- **JWT Bearer Authentication** - Token-based authentication
- **SQLite** - Database (development)
- **PostgreSQL** - Database (production planned)

### DevOps & Cloud

- **Docker** - Containerization
- **AWS** - Cloud platform (ECS Fargate, RDS planned)
- **Railway.app** - Deployment platform

## 📁 Project Structure

```
apps/
├── api/                        # ASP.NET Core Web API
│   └── JobTracker.Api/
│       ├── Controllers/        # API endpoints
│       ├── Data/              # DbContext and configurations
│       ├── DTOs/              # Data Transfer Objects
│       ├── Entities/          # Database models
│       ├── Services/          # Business logic
│       ├── Migrations/        # EF Core migrations
│       └── Program.cs         # Application entry point
│
└── web/                       # Next.js Frontend
    ├── src/
    │   ├── app/              # Next.js App Router pages
    │   ├── components/       # React components
    │   ├── contexts/         # React contexts
    │   └── lib/             # Utilities and helpers
    └── public/              # Static assets
```

## 🚦 Getting Started

### Prerequisites

- **Node.js** (v20 or higher)
- **.NET SDK** (v8.0 or higher)
- **Git**

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/Alexkszzz/job-application-tracker.git
   cd job-application-tracker/apps
   ```

2. **Setup Backend (API)**

   ```bash
   cd api/JobTracker.Api

   # Restore dependencies
   dotnet restore

   # Apply database migrations
   dotnet ef database update

   # Run the API
   dotnet run
   ```

   The API will be available at `http://localhost:5000`

3. **Setup Frontend (Web)**

   ```bash
   cd web

   # Install dependencies
   npm install

   # Run the development server
   npm run dev
   ```

   The web app will be available at `http://localhost:3000`

### Environment Variables

#### Backend (API)

Create an `appsettings.Development.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=jobtracker.db"
  },
  "JwtSettings": {
    "Secret": "your-secret-key-min-32-characters",
    "Issuer": "JobTrackerApi",
    "Audience": "JobTrackerClient",
    "ExpiryInMinutes": 60
  }
}
```

#### Frontend (Web)

Create a `.env.local` file:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## 🧪 Running Tests

### Backend Tests

```bash
cd api/JobTracker.Api.Tests
dotnet test
```

### Frontend Tests

```bash
cd web
npm test              # Run tests once
npm run test:watch    # Run tests in watch mode
npm run test:coverage # Run tests with coverage
```

## 🔧 Development

### API Endpoints

#### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login user

#### Applications

- `GET /api/applications` - Get all applications for authenticated user
- `GET /api/applications/{id}` - Get application by ID
- `POST /api/applications` - Create new application
- `PUT /api/applications/{id}` - Update application
- `DELETE /api/applications/{id}` - Delete application

### Application Status Enum

- `Applied` - Application submitted
- `Interviewing` - In interview process
- `Offered` - Job offer received
- `Rejected` - Application rejected

## 🐳 Docker

Build and run the API with Docker:

```bash
cd api/JobTracker.Api
docker build -t job-tracker-api .
docker run -p 5000:5000 job-tracker-api
```

## 🚀 Deployment

The application is configured for deployment on:

- **Railway.app** - Current deployment platform
- **AWS ECS Fargate** - Planned production deployment
- **AWS RDS PostgreSQL** - Planned production database

## 📝 License

This project is private and not licensed for public use.

## 👤 Author

**Alex** - [Alexkszzz](https://github.com/Alexkszzz)

## 🤝 Contributing

This is a personal project. If you have suggestions or find bugs, feel free to open an issue.

---

Built with ❤️ using Next.js and ASP.NET Core
