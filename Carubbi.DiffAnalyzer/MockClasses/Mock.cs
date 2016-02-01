using System;
using System.Collections.Generic;

namespace Carubbi.DiffAnalyzer.MockClasses
{
    /// <summary>
    /// Classe de negócio fictícia para testar o componente
    /// </summary>
    public class UF
    {
        public string Nome
        {
            get;
            set;
        }

        private Cidade[] _cidades;
        public Cidade[] Cidades
        {
            get
            {
                if (_cidades == null)
                    _cidades = new Cidade[5];

                return _cidades;
            }
            set
            {
                _cidades = value;
            }
        }
    }

    /// <summary>
    /// Classe de negócio fictícia para testar o componente
    /// </summary>
    public class Cidade
    {
        public string Nome
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Classe de negócio fictícia para testar o componente
    /// </summary>
    public class Endereco
    {
        public string Logradouro
        {
            get;
            set;
        }

        public UF UF
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Classe de negócio fictícia para testar o componente
    /// </summary>
    public class Telefone
    {
        public string Numero
        {
            get;
            set;
        }

        public TipoTelefone Tipo
        {
            get;
            set;
        }

        public string DDD
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Classe de negócio fictícia para testar o componente
    /// </summary>
    public enum TipoTelefone : int
    {
        Residencia,
        Comercial,
        Celular,
        Recado
    }

    /// <summary>
    /// Classe de negócio fictícia para testar o componente
    /// </summary>
    public class Usuario
    {
        // Ignora tanto com comportamento CompareAll como CompareMark
        [DiffAnalyzable(DiffAnalyzableUsage.Ignore)]
        public int Id
        {
            get;
            set;
        }

        // Compara em ambos os comportamentos
        [DiffAnalyzable]
        public string Nome
        {
            get;
            set;
        }

        private List<String> _apelidos;

        // propriedade sem atributo compara apenas no comportamento CompareAll
        public List<String> Apelidos
        {
            get
            {
                if (_apelidos == null)
                    _apelidos = new List<String>();

                return _apelidos;
            }
            set
            {
                _apelidos = value;
            }
        }

        private List<Telefone> _telefones;

        [DiffAnalyzable]
        public List<Telefone> Telefones
        {
            get
            {
                if (_telefones == null)
                    _telefones = new List<Telefone>();

                return _telefones;
            }
            set
            {
                _telefones = value;
            }
        }

        private List<Endereco> _enderecos;

        public List<Endereco> Enderecos
        {
            get
            {
                if (_enderecos == null)
                    _enderecos = new List<Endereco>();

                return _enderecos;
            }
            set
            {
                _enderecos = value;
            }
        }
    }
}
