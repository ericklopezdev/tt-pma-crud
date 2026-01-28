import { useState, useEffect } from 'react';
import { DocumentoIdentidad } from '../types';
import { documentService } from '../services/documentService';

export const useDocumentTypes = () => {
  const [documents, setDocuments] = useState<DocumentoIdentidad[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchDocuments = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await documentService.getActiveDocuments();
      setDocuments(data);
    } catch (err) {
      setError('Error al cargar los tipos de documento');
      console.error('Error fetching documents:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDocuments();
  }, []);

  return {
    documents,
    loading,
    error,
    refetch: fetchDocuments
  };
};