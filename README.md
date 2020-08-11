# WeChipCredit

* Adicionada Base clientes e Ofertas Inicial para realização dos testes/ Sistema sobe com alguns clientes iniciais. 
* Api e Forms estão em mesma solução.
* Passos abaixo para Testes de cada um Isoladamente.

# Testar Api: 
	Expandir Solução WeChipCredit > Clicar com botão direito em Api_WebApplication > Definir como Projeto de inicialização > Depurar IIS EXPRESS 
	...api/Offers o retorno de todas as ofertas cadastradas.
	...api/Offers?name=da o retorno de todas as ofertas cadastradas com parte ou nome completo inserido após o "=".
	
# Devido o sistema não efetuar consulta ao banco de dados para atualização da lista de ofertas não é possivel acompanhar as propostas novas cadastradas pelo software, porem lancei algumas propostas e Clientes Fakes para teste. 

# Testar Software: 
	Expandir Solução WeChipCredit > Clicar com botão direito em WeChipCredit > Definir como Projeto de inicialização > Depurar
	
# Sobre Base de Clientes e Ofertas Fakes para facilitar os testes. 
	O sistema é completamente funcional e pode-se adicionar mais clientes, deletar e efetuar os demais processos escopados... A base adicionada não interfere em nenhum ponto de funcionamento, e é utilizada somente para facilitar os testes, pois o mesmo não possui CRUD com nenhum BD então ele carregaria completamente sem base de cliente inicial ou ofertas.
	
