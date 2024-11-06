CREATE TABLE "user"
(
    user_id    SERIAL PRIMARY KEY,
    email      VARCHAR(255)              NOT NULL UNIQUE,
    password   VARCHAR(255)              NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW() NOT NULL
);