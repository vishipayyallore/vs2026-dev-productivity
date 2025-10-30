-- VS2026 Dev Productivity PostgreSQL Database Initialization
-- This script runs when the PostgreSQL container starts for the first time

-- Create the productdb database (if not exists)
-- Note: POSTGRES_DB environment variable already creates this, but this is here for completeness
-- SELECT 'CREATE DATABASE productdb' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'productdb')\gexec

-- Create any additional schemas, users, or initial setup as needed
-- For now, we'll let EF Core migrations handle the schema

-- Log successful initialization
\echo 'VS2026 PostgreSQL database initialized successfully'