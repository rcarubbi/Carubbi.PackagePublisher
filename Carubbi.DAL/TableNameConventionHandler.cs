
namespace Carubbi.DAL
{
    /// <summary>
    /// Delegate que recebe um método responsável por definir regras nos nomes das tabelas a partir do nome das entidades
    /// </summary>
    /// <param name="entityName">nome da entidade</param>
    /// <returns>nome da tabela</returns>
    public delegate string EntityNameConventionHandler(string entityName);
}
