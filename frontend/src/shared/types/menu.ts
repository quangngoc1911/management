export interface Menu {
  id: number;
  name: string;
  path: string;
  icon: string;
  order: number;
  isVisible: boolean;
  parentId: number | null;
  children: Menu[];
}

export interface CreateMenuRequest {
  name: string;
  path: string;
  icon?: string;
  order?: number;
  isVisible?: boolean;
  parentId?: number | null;
}