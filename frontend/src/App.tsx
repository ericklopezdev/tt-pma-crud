import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from './theme/ThemeProvider';
import { BeneficiariosPage } from './pages/BeneficiariosPage';
import { ThemeToggle } from './components/ThemeToggle';
import { Toaster } from 'react-hot-toast';

function App() {
  return (
    <ThemeProvider>
      <Router>
        <div className="min-h-screen dark:bg-mesh-gradient-dark text-slate-900 dark:text-slate-100 transition-colors duration-300">
          <header className="sticky top-0 z-50 glass dark:glass-dark border-b border-white/20 dark:border-slate-800/50">
            <div className="container mx-auto px-4 py-3">
              <div className="flex justify-between items-center">
                <div className="flex items-center gap-2">
                  <h1 className="text-xl font-bold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-primary-600 to-primary-400">
                    Beneficiarios
                  </h1>
                </div>
                <ThemeToggle />
              </div>
            </div>
          </header>

          <main className="container mx-auto px-4 py-8 animate-fade-in">
            <Routes>
              <Route path="/" element={<BeneficiariosPage />} />
            </Routes>
          </main>

          <Toaster
            position="bottom-center"
            toastOptions={{
              className: 'dark:bg-slate-800 dark:text-white rounded-2xl border border-white/10 shadow-2xl backdrop-blur-md',
              duration: 4000,
            }}
          />
        </div>
      </Router>
    </ThemeProvider>
  );
}

export default App;