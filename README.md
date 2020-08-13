# WeChipCredit

* Adicionada Base Clientes e Ofertas inicial para realização dos testes de Api e do Sistema.
* Api e Software estão na mesma solução.
* Passos abaixo para Testes de cada um Isoladamente.

# Testar Api: 
	Expandir Solução WeChipCredit > Clicar com botão direito em Api_WebApplication > Definir como Projeto de inicialização > Depurar IIS EXPRESS 
	GET: /api/Offers  O retorno de todas as ofertas cadastradas.
	GET: /api/Offers?name=da  Retorna ofertas com parte ou nome completo inserido.
	GET: /api/Offers?cpf=402  Retorna ofertas com parte ou todo o CPF, Recebe somente numeros.
	
Ex: https://localhost:44338/api/Offers?name=da

# Testar Software: 
	Expandir Solução WeChipCredit > Clicar com botão direito em WeChipCredit > Definir como Projeto de inicialização > Depurar
	
# Sobre Base de Clientes e Ofertas Fakes para facilitar os testes. 
	Devido o sistema não efetuar consulta ao banco de dados para resgate e armazenamento das informações trafegadas dentro do software e não dser possível acompanhar as propostas registradas anteriormente a mesma foi adicionada para facilitar os testes.  A base adicionada não interfere em nenhum ponto de funcionamento, e é utilizada somente com esse fim principalmente para teste da API.
	
	
