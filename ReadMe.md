# Minibus Taxi Route Tracker

## 1. Overview
The **Minibus Taxi Route Tracker** is a lightweight CRUD-based API designed to capture and manage information about South African taxi routes, their stops, fare structures, and operational incidents.  
It aims to bring structure and visibility to informal transport data while demonstrating clean architecture principles using **.NET 8**, **Dapper**, and **MySQL**.

---

## 2. Problem Statement
In many South African cities, the **minibus taxi system** is the backbone of daily commuting, yet information about routes, stops, fares, and disruptions is often informal, fragmented, or unavailable to the public.  
This lack of structured data leads to:
- Commuters being unaware of updated fares or active incidents.
- Difficulty for municipal or community planners to analyze transport trends.
- Missed opportunities for data-driven improvements and commuter safety.

The goal of this project is to build a simple, structured system that records and exposes this information via APIs — serving as a foundation for future commuter apps, dashboards, or reports.

---

## 3. Objectives
- Provide CRUD endpoints for Associations, Routes, Stops, and Incidents.
- Demonstrate **clean architecture layering**:
  - **Presentation** (Web API)
  - **Application** (Business logic)
  - **Domain** (Entities + validation)
  - **Infrastructure** (Dapper repositories + MySQL access)
- Implement Dapper mapping using the custom `DapperHelper.UsePropertyAttributeMapping<T>()`.
- Keep the solution extensible for additional modules (FareHistory, GPS, etc.).

---

## 4. System Scope
**In-Scope Features**
- Manage **Taxi Associations** and their Routes.
- Manage **Stops** per Route with correct ordering.
- Record **Incidents** (protests, fare changes, closures, etc.) per Route.
- Retrieve data via RESTful endpoints.

**Out of Scope (for MVP)**
- Authentication or user management.
- Real-time updates or push notifications.
- Geolocation mapping or mobile front-end.

---

## 5. Entities and Relationships
| Entity | Description | Relationships |
|--------|--------------|----------------|
| **Association** | Represents a taxi association or operator. | One-to-Many → Routes |
| **Route** | Describes a taxi route, including fare details. | Belongs to Association, has many Stops and Incidents |
| **Stop** | A physical pickup/drop-off point along a route. | Belongs to Route |
| **Incident** | Records issues or updates affecting a route. | Belongs to Route |