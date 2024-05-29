export class User {
  id: string = '';
  name: string = '';
  email: string = '';
  avatar: string = '';
  isBlocked: boolean = false;
  phoneNumber: string = '';
  role: string = '';
  registerTime: string = '';
  lastSignIn: string = '';
  selected: boolean = false;
}

export interface UserFilter {
  pageIndex: number;
  pageSize: number;
  sortBy: string;
  ascSort: boolean;
  keyword: string;
  isBlocked: boolean | null;
  role: string;
}
