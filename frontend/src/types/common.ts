export interface ThemeContextType {
  isDark: boolean;
  toggleTheme: () => void;
}

export interface ValidationError {
  property: string;
  error: string;
}

export interface ApiResponse<T> {
  data?: T;
  error?: string;
  errors?: ValidationError[];
}