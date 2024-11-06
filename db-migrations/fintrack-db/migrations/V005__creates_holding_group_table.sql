CREATE TABLE holding_group (
    holding_group_id SERIAL PRIMARY KEY,
    user_id          INTEGER                   NOT NULL,
    name             VARCHAR(127)              NOT NULL,
    created_at       TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    updated_at       TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "user" (user_id) ON DELETE CASCADE
);

CREATE OR REPLACE FUNCTION update_updated_at_column_holding_group()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_holding_group_updated_at
    BEFORE UPDATE
    ON holding_group
    FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column_holding_group();