# Обработчик аргументов консольного приложения

## Пример использования.

1. В файле точки входа в программу (например, Program.cs) создается переменная типа **ArgumentsCollection**. Это основное хранилище и обработчик аргументов.
```
class Program
    {
        private static ArgumentsCollection _arguments
        ...
    }
```
2. Перед тем, как начать обработку аргументов, в конструкторе или в методе Main заполняется коллекция допустимых аргументов:
```
_arguments = new ArgumentsCollection();
_arguments.AddParameter(new InputArgument("-v", "Verbose.", () => _verbose = true, false);
_arguments.AddParameter(new InputArgumentWithInput("-f", "Connection string filename.", (string path) => _connectionString = path, true, "-c");
_arguments.AddParameter(new InputArgumentWithInput("-c", "Connection string.", (string connString) => _connectionString = connString, true, "-f");
```
Обработчики добавляются через метод **AddParameter**. Метод принимает типы InputArgument (аргумент без значения) и InputArgumentWithInput (аргумент со значением). Сигнатура: AddParameter(InputArgumentBase par, bool required = false, params string[] aliases). Параметр **required** сообщает, является ли аргумент обязательным. **aliases** перечисляет альтернативные аргументы, которые можно использовать вместо указанного. В примере выше два аргумента -f и -c являются обязательными, но использовать допускается только один из них. Либо указать файл со сторокой подключения, либо написать строку подключения в терминале и передать как значение аргумента.

Конструкторы InputArgument и InputArgumentWithInput:
- InputArgument(string name, string descr, Action process)
- InputArgumentWithInput(string name, string descr, Action<string> process)

3. После подготовки коллекции обработчиков запускатся сама обработка аргументов с помощью метода **ProcessArguments**:
```
       private static void Main(string[] args)
       {
           ...
           _arguments = new ArgumentsCollection();
           ...
           _arguments.ProcessArguments(args);
       }
```
Если в процессе обработки что-то пойдет не так во время перебора аргументов (исключения лямбда-функций не обрабатываются), тогда консольное приложение завершится с кодом 1 и будет отображена подсказка. Подсказку по аргументам также можно получить явно с помощью передачи программе агумента **-h**.
