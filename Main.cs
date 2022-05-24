class Program {
    static void Main(string[] args) {
        FileHandler handler = new FileHandler();
        handler.BuildFile();
        handler.CreateInvertedFiles();
        List<Conta> list = new List<Conta>();

        while (true) {
            Console.Clear();
            switch (Menu()) {

                case 1: {
                    Divisor();
                    Console.WriteLine("ABERTURA DE CONTA\nNome: ");
                    string nomePessoa = Console.ReadLine();
                    Console.Write("cpf: ");
                    string cpf = Console.ReadLine();
                    Console.Write("Cidada: ");
                    string cidade = Console.ReadLine();
                    Conta contaNova = new Conta(nomePessoa, cpf, cidade);
                    fh.Create(contaNova);
                    Console.WriteLine("\nConta criada!\n");
                    Console.WriteLine(contaNova.ToString());
                    Thread.Sleep(5000);
                    break;
                }

                case 2: {
                    Divisor();
                    Console.Write("ID da sua conta: ");
                    ushort id = ushort.Parse(Console.ReadLine());

                    Conta? conta = fh.ReadByPos(fh.FindPosByIndex(id));

                    if (conta != null) {
                        Divisor();
                        Console.Write("Quantia a ser depositada: ");
                        conta.Depositar(float.Parse(Console.ReadLine()));

                        fh.UpdateById(conta, id);
                    } else {
                        Console.WriteLine("\nEsta conta não existe!\n");
                    }
                    break;
                }

                case 3: {
                    Divisor();
                    Console.Write("ID da sua conta: ");
                    ushort id1 = ushort.Parse(Console.ReadLine());

                    Console.Write("ID da conta à receber a transferência: ");
                    ushort id2 = ushort.Parse(Console.ReadLine());

                    Console.Write("Quanto você deseja transferir: ");
                    float transf = float.Parse(Console.ReadLine());

                    Conta? conta1 = handler.ReadByPos(handler.FindPosByIndex(id1));
                    Conta? conta2 = handler.ReadByPos(handler.FindPosByIndex(id2));

                    if (conta1 != null && conta2 != null) {
                        if (conta1.SaldoConta >= transf) {
                            conta1.Transferir(transf);
                        }

                        conta2.Depositar(transf);

                        handler.UpdateById(conta1, id1);
                        handler.UpdateById(conta2, id2);
                    } else {
                        Console.WriteLine("\nERRO!\n");
                    }
                    break;
                }

                case 4: {
                    Divisor();
                    Console.Write("Deseja fazer a busca por Cidade(0), Nome(1) ou ID(2)?: ");
                    ushort? op = ushort.Parse(Console.ReadLine());

                    if (op == null) {
                        Console.WriteLine("\nOperação não encontrada!\n");
                        Thread.Sleep(2000);
                        break;
                    }
                    imprimeCidade(op); 
                    Thread.Sleep(5000);
                    break;
                }

                case 5: {
                    Divisor();
                    Console.Write("Qual ID da conta você deseja atualizar?: ");
                    ushort id = ushort.Parse(Console.ReadLine());

                    Conta? conta = handler.ReadById(id);

                    if (conta == null) {
                        Console.WriteLine("\nO ID informado não pertence a nenhuma conta!\n");
                        Thread.Sleep(4000);
                        break;
                    }

                    Console.Write("Nome da conta: ");
                    string nome = Console.ReadLine();

                    Console.Write("cpf da conta.: ");
                    string cpf = Console.ReadLine();

                    Console.Write("Cidade da conta: ");
                    string cidade = Console.ReadLine();

                    if (nome != null && cpf != null && cidade != null) {
                        Console.WriteLine("\nAntes da atualização:\n");
                        Console.WriteLine(conta.ToString());

                        conta.NomePessoa = nome; conta.cpf = cpf; conta.Cidade = cidade;
                        handler.UpdateById(conta, id);

                        Console.WriteLine("\nDepois da atualização:\n");
                        Console.WriteLine(conta.ToString());
                        Thread.Sleep(5000);
                    } else {
                        Console.WriteLine("\nFalta de informações para criar conta!\n");
                        Thread.Sleep(2000);
                    }

                    break;
                }

                case 6: {
                    Divisor();
                    Console.Write("Qual ID da conta você deseja deletar?: ");
                    ushort id = ushort.Parse(Console.ReadLine());

                    if (handler.DeleteById(id))
                        Console.WriteLine($"\nConta de ID {id} deletada com sucesso!\n");
                    else
                        Console.WriteLine($"\nA conta com o ID {id} não existe\n");
                    Thread.Sleep(5000);
                    break;
                }

                case 0: {
                    Console.Clear();
                    Console.Write("Finalizado ");
                    Console.ReadLine();
                    break;
                }

                default: {
                    break;
                }
            }
        }

    }

    public static int Menu() {
        Console.Clear();
        Console.Write(
            "1. Abrir conta\n" +
            "2. Depositar\n" +
            "3. Transferir\n" +
            "4. Ver registro\n" +
            "5. Atualizar registro\n" +
            "6. Deletar registro\n" +
            "0. Sair\n" +
            "\n: ");
        return int.Parse(Console.ReadLine());
    }

  
    public static void Divisor() {
        Console.WriteLine("\n---------------------------------------------\n");
    }

    public static void imprimeCidade(string op){
        switch (op) {
            case 0:
                Console.Write("Digite o nome da cidade> ");
                string? cidade = Console.ReadLine();

                if (cidade == null) {
                    Console.WriteLine("\nOperação não encontrada!\n");
                    Thread.Sleep(2000);
                    break;
                }

                list = handler.ReadByCity(cidade);

                if (list.Count > 0) {
                    for (int i = 0; i < list.Count; i++) {
                        if (!list.ElementAt(i).Lapide) {
                            Console.WriteLine("\n" + list.ElementAt(i));
                        } else if (list.ElementAt(i) == null) {
                            Console.WriteLine("\nConta não encontrada!");
                        } else {
                            Console.WriteLine("\nConta foi excluída!");
                        }
                    }
                } else {
                    Console.WriteLine("\nItem não ecnontrado!\n");
                }
                break;

            case 1:
                Console.Write("Digite o nome desejado: ");
                string? nome = Console.ReadLine();

                if (nome == null) {
                    Console.WriteLine("\nOperação não encontrada!\n");
                    Thread.Sleep(2000);
                    break;
                }

                list = handler.ReadByName(nome);
                if (list.Count > 0) {
                    for (int i = 0; i < list.Count; i++) {
                        if (!list.ElementAt(i).Lapide) {
                            Console.WriteLine("\n" + list.ElementAt(i));
                        } else if (list.ElementAt(i) == null) {
                            Console.WriteLine("\nConta não existe!");
                        } else {
                            Console.WriteLine("\nConta foi excluída!");
                        }
                    }
                } else {
                    Console.WriteLine("\nNenhum item foi encontrado com este nome!\n");
                }
                break;

            case 2:
                Console.Write("Insira o id: ");
                ushort? id = ushort.Parse(Console.ReadLine());
                long pos = handler.FindPosByIndex(id.GetValueOrDefault());

                Conta? conta = handler.ReadByPos(pos);

                if (conta != null) {
                    if (!conta.Lapide) {
                        Console.WriteLine("\n" + conta);
                    } else {
                        Console.WriteLine("\nExcluída com sucesso!");
                    }
                } else {
                    Console.WriteLine("\nA não existe!");
                }
                break;

            default:
                Console.WriteLine("Opção inválida!");
                break;
        }
    }
}
