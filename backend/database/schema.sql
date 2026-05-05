-- =====================================================
-- DOCUMENT RETRIEVAL SYSTEM
-- PostgreSQL Enterprise Schema (UUID Version)
-- .NET EF Core Compatible
-- Re-runnable
-- Admin password = 123456 (BCrypt)
-- =====================================================

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- =====================================================
-- DROP ALL TABLES
-- =====================================================

DROP TABLE IF EXISTS document_tags CASCADE;
DROP TABLE IF EXISTS bookmarks CASCADE;
DROP TABLE IF EXISTS view_histories CASCADE;
DROP TABLE IF EXISTS refresh_tokens CASCADE;
DROP TABLE IF EXISTS document_versions CASCADE;
DROP TABLE IF EXISTS documents CASCADE;
DROP TABLE IF EXISTS files CASCADE;
DROP TABLE IF EXISTS tags CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS system_configs CASCADE;
DROP TABLE IF EXISTS users CASCADE;

-- =====================================================
-- USERS
-- =====================================================

CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    name VARCHAR(150) NOT NULL,
    email VARCHAR(256) NOT NULL UNIQUE,
    password_hash VARCHAR(500) NOT NULL,

    role VARCHAR(30) NOT NULL DEFAULT 'Viewer'
        CHECK (role IN ('Admin','Editor','Viewer')),

    avatar_url VARCHAR(500),
    department VARCHAR(100),

    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    last_login_at TIMESTAMP NULL,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,

    created_by UUID NULL,
    updated_by UUID NULL,

    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at TIMESTAMP NULL,
    deleted_by UUID NULL
);

CREATE INDEX ix_users_email ON users(email);
CREATE INDEX ix_users_role ON users(role);
CREATE INDEX ix_users_active ON users(is_active);

-- =====================================================
-- CATEGORIES
-- =====================================================

CREATE TABLE categories (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    name VARCHAR(200) NOT NULL,
    slug VARCHAR(200) NOT NULL UNIQUE,

    description VARCHAR(1000),
    icon VARCHAR(100),
    cover_image_url VARCHAR(500),

    parent_id UUID NULL REFERENCES categories(id) ON DELETE RESTRICT,

    sort_order INT NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,

    created_by UUID NULL,
    updated_by UUID NULL,

    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at TIMESTAMP NULL,
    deleted_by UUID NULL
);

CREATE INDEX ix_categories_slug ON categories(slug);
CREATE INDEX ix_categories_parent ON categories(parent_id);

-- =====================================================
-- FILES
-- =====================================================

CREATE TABLE files (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    original_name VARCHAR(500) NOT NULL,
    stored_name VARCHAR(500) NOT NULL,
    storage_path VARCHAR(1000) NOT NULL,
    public_url VARCHAR(1000) NOT NULL,

    file_type VARCHAR(50) NOT NULL,
    mime_type VARCHAR(100) NOT NULL,
    size_bytes BIGINT NOT NULL CHECK (size_bytes >= 0),

    uploaded_by_user_id UUID NOT NULL REFERENCES users(id),

    storage_provider VARCHAR(30) NOT NULL DEFAULT 'local',

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,

    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at TIMESTAMP NULL
);

-- =====================================================
-- DOCUMENTS
-- =====================================================

CREATE TABLE documents (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    title VARCHAR(500) NOT NULL,
    slug VARCHAR(500) NOT NULL UNIQUE,

    summary VARCHAR(2000),

    content_type VARCHAR(30) NOT NULL DEFAULT 'text'
        CHECK (content_type IN ('text','html','markdown','file','pdf')),

    content TEXT,

    file_id UUID NULL REFERENCES files(id) ON DELETE SET NULL,
    thumbnail_url VARCHAR(500),

    category_id UUID NOT NULL REFERENCES categories(id),
    created_by_user_id UUID NOT NULL REFERENCES users(id),

    is_published BOOLEAN NOT NULL DEFAULT FALSE,
    published_at TIMESTAMP NULL,

    view_count INT NOT NULL DEFAULT 0,
    sort_order INT NOT NULL DEFAULT 0,
    version INT NOT NULL DEFAULT 1,

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,

    created_by UUID NULL,
    updated_by UUID NULL,

    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at TIMESTAMP NULL,
    deleted_by UUID NULL
);

CREATE INDEX ix_documents_slug ON documents(slug);
CREATE INDEX ix_documents_category ON documents(category_id);
CREATE INDEX ix_documents_published ON documents(is_published);
CREATE INDEX ix_documents_created_user ON documents(created_by_user_id);
CREATE INDEX ix_documents_lookup
ON documents(category_id, is_published, is_deleted);

-- =====================================================
-- TAGS
-- =====================================================

CREATE TABLE tags (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) NOT NULL UNIQUE,
    color VARCHAR(7),

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,

    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    deleted_at TIMESTAMP NULL
);

CREATE INDEX ix_tags_slug ON tags(slug);

-- =====================================================
-- DOCUMENT TAGS
-- =====================================================

CREATE TABLE document_tags (
    document_id UUID NOT NULL REFERENCES documents(id) ON DELETE CASCADE,
    tag_id UUID NOT NULL REFERENCES tags(id) ON DELETE CASCADE,

    added_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY(document_id, tag_id)
);

-- =====================================================
-- DOCUMENT VERSIONS
-- =====================================================

CREATE TABLE document_versions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    document_id UUID NOT NULL REFERENCES documents(id) ON DELETE CASCADE,

    version_number INT NOT NULL,
    title VARCHAR(500) NOT NULL,
    content TEXT,

    change_summary VARCHAR(500),

    edited_by_user_id UUID NOT NULL REFERENCES users(id),

    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UNIQUE(document_id, version_number)
);

CREATE INDEX ix_doc_versions_doc ON document_versions(document_id);

-- =====================================================
-- BOOKMARKS
-- =====================================================

CREATE TABLE bookmarks (
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    document_id UUID NOT NULL REFERENCES documents(id) ON DELETE CASCADE,

    note VARCHAR(500),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY(user_id, document_id)
);

-- =====================================================
-- VIEW HISTORIES
-- =====================================================

CREATE TABLE view_histories (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    document_id UUID NOT NULL REFERENCES documents(id) ON DELETE CASCADE,

    viewed_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    duration_sec INT NULL CHECK (duration_sec >= 0)
);

CREATE INDEX ix_view_history_user_time
ON view_histories(user_id, viewed_at DESC);

-- =====================================================
-- REFRESH TOKENS
-- =====================================================

CREATE TABLE refresh_tokens (
    id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id uuid NOT NULL REFERENCES users(id) ON DELETE CASCADE,

    token text NULL,
    token_hash varchar(500) NOT NULL,

    device_info varchar(300),
    created_by_ip varchar(100),

    expires_at timestamp NOT NULL,
    is_revoked boolean NOT NULL DEFAULT false,

    created_at timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp NULL,

    created_by uuid NULL,
    updated_by uuid NULL,

    is_deleted boolean NOT NULL DEFAULT false,
    deleted_at timestamp NULL,
    deleted_by uuid NULL
);

CREATE INDEX ix_refresh_tokens_user ON refresh_tokens(user_id);

-- =====================================================
-- SYSTEM CONFIGS
-- =====================================================

CREATE TABLE system_configs (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    key VARCHAR(100) NOT NULL UNIQUE,
    value TEXT,
    description VARCHAR(300),

    is_public BOOLEAN NOT NULL DEFAULT FALSE,

    updated_at TIMESTAMP NULL
);

-- =====================================================
-- SEED USERS
-- password = 123456
-- BCrypt Hash
-- =====================================================

INSERT INTO users(name,email,password_hash,role,is_active)
VALUES
(
'Super Admin',
'admin@system.com',
'$2a$11$UmvYYQpOJYOtDXbH5zivQe0bmXoz.OFECFcC1QDZsYk2N5cSa.Ndu',
'Admin',
TRUE
),
(
'Editor Demo',
'editor@system.com',
'$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYqKx8pX/Wm',
'Editor',
TRUE
),
(
'Viewer Demo',
'viewer@system.com',
'$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYqKx8pX/Wm',
'Viewer',
TRUE
);

-- =====================================================
-- SEED CATEGORIES
-- =====================================================

INSERT INTO categories(name,slug,description,icon,sort_order,is_active)
VALUES
('Tài liệu','tai-lieu','Danh mục gốc','folder',0,TRUE),
('Hướng dẫn','huong-dan','Tài liệu hướng dẫn','book',1,TRUE),
('Quy trình','quy-trinh','Quy trình nội bộ','git-branch',2,TRUE),
('Biểu mẫu','bieu-mau','Mẫu văn bản','file-text',3,TRUE);

-- =====================================================
-- SEED TAGS
-- =====================================================

INSERT INTO tags(name,slug,color)
VALUES
('Quan trọng','quan-trong','#DC2626'),
('Mới','moi','#2563EB'),
('Cập nhật','cap-nhat','#16A34A'),
('Khẩn cấp','khan-cap','#EA580C');

-- =====================================================
-- SEED CONFIGS
-- =====================================================

INSERT INTO system_configs(key,value,description,is_public)
VALUES
('system.name','Document Retrieval System','Tên hệ thống',TRUE),
('pagination.default_page_size','20','Phân trang mặc định',TRUE),
('file.max_size_mb','50','Giới hạn upload',TRUE);