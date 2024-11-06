CREATE TABLE user_session
(
    session_token VARCHAR(63) PRIMARY KEY,
    user_id       INTEGER   NOT NULL,
    is_active     BOOLEAN   NOT NULL DEFAULT TRUE,
    emitted_at    TIMESTAMP NOT NULL DEFAULT NOW(),
    expires_at    TIMESTAMP NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "user" (user_id) ON DELETE CASCADE
);