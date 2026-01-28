import React from 'react';
import { useTheme } from '../theme/ThemeProvider';
import { Sun, Moon } from 'lucide-react';

interface ThemeToggleProps {
  className?: string;
}

export const ThemeToggle: React.FC<ThemeToggleProps> = ({ className = '' }) => {
  const { isDark, toggleTheme } = useTheme();

  return (
    <button
      onClick={toggleTheme}
      className={`p-2.5 rounded-xl transition-all duration-300 active:scale-95 ${isDark
          ? 'bg-slate-800 text-amber-400 hover:bg-slate-700 shadow-lg border border-slate-700'
          : 'bg-white text-slate-800 hover:bg-slate-50 shadow-md border border-slate-200'
        } ${className}`}
      aria-label="Cambiar tema"
    >
      {isDark ? <Sun size={20} /> : <Moon size={20} />}
    </button>
  );
};
