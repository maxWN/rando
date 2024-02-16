namespace Rando.Scripts
{
    public static class MySqlQueries
    {
        /// <summary>
        /// Bank Table Creation Query
        /// <para>Required Arguments:</para>
        /// <para>First argument represents Database Name and second argument represents the Table name</para>
        /// </summary>
        public const string CREATE_BANKS_TABLE = "CREATE TABLE IF NOT EXISTS `{0}`.`{1}` (\r\n  `id` INT NOT NULL,\r\n  `uid` VARCHAR(45) NULL,\r\n  `account_number` VARCHAR(30) NOT NULL,\r\n  `iban` VARCHAR(30) NULL,  \r\n  `bank_name` VARCHAR(60) NOT NULL,\r\n  `routing_number` VARCHAR(15) NOT NULL,\r\n  `swift_bic` VARCHAR(15) NOT NULL,\r\n  PRIMARY KEY (`id`),\r\n  UNIQUE INDEX `uid_UNIQUE` (`uid` ASC) VISIBLE,\r\n  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE)\r\nCOMMENT = 'Create table if doesn\\'t already exist';";

        /// <summary>
        /// Beer Table Creation Query
        /// <para>Required Arguments:</para>
        /// <para>First argument represents Database Name and second argument represents the Table name</para>
        /// </summary>
        public const string CREATE_BEERS_TABLE = "CREATE TABLE IF NOT EXISTS `{0}`.`{1}` (\r\n  `id` INT NOT NULL,\r\n  `uid` VARCHAR(40) NULL,\r\n  `brand` VARCHAR(45) NULL,\r\n  `name` VARCHAR(45) NULL,\r\n  `style` VARCHAR(45) NULL,\r\n  `hop` VARCHAR(20) NULL,\r\n  `yeast` VARCHAR(20) NULL,\r\n  `malts` VARCHAR(20) NULL,\r\n  `ibu` VARCHAR(20) NULL,\r\n  `alcohol` DECIMAL(10) NULL,\r\n  `blg` VARCHAR(20) NULL,\r\n  PRIMARY KEY (`id`),\r\n  UNIQUE INDEX `uid_UNIQUE` (`uid` ASC) VISIBLE);";
    }
}