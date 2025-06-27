# PurchasingOrder

This repository implements the Purchase Order (PO) bounded context of a small ERP system, focusing on the purchasing process. It follows Domain-Driven Design (DDD) principles and is part of an assessment task.

## Overview
- **Bounded Context**: Purchase Orders (POs)  
- **Purpose**: Manage the creation, state transitions, and items of POs.  
- **Technology Stack**: .NET, EF Core, Microsoft SQL Server, Docker  
- **Single Branch**: `main` (all work, including incomplete, will reside here)  

## Entities
- **PurchaseOrder**  
  - Number (unique, generated with configurable logic)  
  - DateIssued  
  - TotalPrice  
  - State (Created, Approved, Shipped, Closed)
  - Status
  - Items (list of PurchaseOrderItem)  
- **PurchaseOrderItem**  
  - SerialNumber  
  - GoodCode  
  - Price  

## Requirements
- Standalone process, containerized with Docker.  
- Owns its data, stored in a Microsoft SQL Server database.  
- Communicates reactively with the ShippingOrder context for state updates.  

## Setup Instructions
1. Clone the repository: `git clone https://github.com/ERPAssessment/PurchasingOrder.git`  
2. Navigate to the directory: `cd PurchasingOrder`  
3. Further setup (Docker, database, etc.) will be added as implementation progresses.
