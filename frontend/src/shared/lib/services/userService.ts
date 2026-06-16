import { createCrudService } from './createCrudService';
import type {
    CreateUserRequest,
    UpdateUserRequest,
    User,
    UserQuery,
} from '@/features/users/types/user';

// Types live in the feature layer; re-exported here for existing import sites.
export type {
    User,
    CreateUserRequest,
    UpdateUserRequest,
    UserQuery,
    PaginatedUsers,
} from '@/features/users/types/user';

// Built from the shared factory — removes the per-method `if (success) ... throw` boilerplate.
// Backend route: api/users.
export const userService = createCrudService<User, CreateUserRequest, UpdateUserRequest, UserQuery>(
    '/users',
);

export default userService;
