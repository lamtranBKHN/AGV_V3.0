-- --------------------------------------------------------
-- Host:                         10.116.42.133
-- Server version:               10.3.29-MariaDB-0ubuntu0.20.04.1 - Ubuntu 20.04
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             11.2.0.6213
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for agv
CREATE DATABASE IF NOT EXISTS `agv` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `agv`;

-- Dumping structure for table agv.agv
CREATE TABLE IF NOT EXISTS `agv` (
  `id` smallint(6) NOT NULL DEFAULT 0,
  `ip` varchar(20) NOT NULL DEFAULT '0',
  `port` smallint(6) NOT NULL DEFAULT 0,
  `cam_ip` varchar(20) DEFAULT NULL,
  `orientation` smallint(6) DEFAULT NULL,
  `current_POD` smallint(6) DEFAULT NULL,
  `batt` smallint(6) DEFAULT NULL,
  `status` tinyint(4) DEFAULT NULL,
  `node_id` smallint(6) DEFAULT NULL,
  `isAvailable` tinyint(1) unsigned zerofill NOT NULL,
  `speed` int(5) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `node_id` (`node_id`),
  KEY `FK_agv_POD` (`current_POD`),
  CONSTRAINT `FK1_agv_nodeId` FOREIGN KEY (`node_id`) REFERENCES `map` (`id`),
  CONSTRAINT `FK2_agv_pod` FOREIGN KEY (`current_POD`) REFERENCES `pod` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table agv.agv: ~4 rows (approximately)
DELETE FROM `agv`;
/*!40000 ALTER TABLE `agv` DISABLE KEYS */;
INSERT INTO `agv` (`id`, `ip`, `port`, `cam_ip`, `orientation`, `current_POD`, `batt`, `status`, `node_id`, `isAvailable`, `speed`) VALUES
	(1, '192.168.1.88', 1000, '192.168.1.24', 0, 0, 50, 1, 36, 1, 80),
	(2, '192.168.1.88', 1001, '192.168.1.25', 270, 0, 80, 1, 85, 0, 60),
	(3, '192.168.1.88', 1002, '192.168.1.26', 180, 0, 90, 1, 32, 0, 30),
	(4, '192.168.1.88', 1002, '192.168.1.27', 90, 0, 90, 0, 34, 1, 30);
/*!40000 ALTER TABLE `agv` ENABLE KEYS */;

-- Dumping structure for table agv.barrier
CREATE TABLE IF NOT EXISTS `barrier` (
  `id` smallint(6) NOT NULL DEFAULT 0,
  `node_id` smallint(6) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `node_id` (`node_id`),
  CONSTRAINT `FK1_barrier_nodeId` FOREIGN KEY (`node_id`) REFERENCES `map` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table agv.barrier: ~4 rows (approximately)
DELETE FROM `barrier`;
/*!40000 ALTER TABLE `barrier` DISABLE KEYS */;
INSERT INTO `barrier` (`id`, `node_id`) VALUES
	(1, 0),
	(2, 9),
	(4, 80),
	(3, 89);
/*!40000 ALTER TABLE `barrier` ENABLE KEYS */;

-- Dumping structure for table agv.charger
CREATE TABLE IF NOT EXISTS `charger` (
  `id` smallint(6) NOT NULL DEFAULT 0,
  `node_id` smallint(6) DEFAULT NULL,
  `orientation` smallint(6) DEFAULT NULL,
  `charging_status` smallint(6) DEFAULT NULL,
  `agv_id` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `node_id` (`node_id`),
  CONSTRAINT `FK1_charger_nodeId` FOREIGN KEY (`node_id`) REFERENCES `map` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table agv.charger: ~2 rows (approximately)
DELETE FROM `charger`;
/*!40000 ALTER TABLE `charger` DISABLE KEYS */;
INSERT INTO `charger` (`id`, `node_id`, `orientation`, `charging_status`, `agv_id`) VALUES
	(1, 8, 270, 0, 0),
	(2, 19, 180, 0, 0);
/*!40000 ALTER TABLE `charger` ENABLE KEYS */;

-- Dumping structure for table agv.map
CREATE TABLE IF NOT EXISTS `map` (
  `id` smallint(6) NOT NULL DEFAULT 0,
  `rfid_value` varchar(50) NOT NULL DEFAULT '',
  `x` smallint(6) DEFAULT NULL,
  `y` mediumint(9) DEFAULT NULL,
  `is_blocked` tinyint(4) DEFAULT NULL,
  `is_active` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `rfid_value` (`rfid_value`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table agv.map: ~90 rows (approximately)
DELETE FROM `map`;
/*!40000 ALTER TABLE `map` DISABLE KEYS */;
INSERT INTO `map` (`id`, `rfid_value`, `x`, `y`, `is_blocked`, `is_active`) VALUES
	(0, 'C9 45 FE E0', 0, 0, 1, 1),
	(1, '69 EE B2 68', 1, 0, 0, 1),
	(2, '1D 9C 85 2E', 2, 0, 0, 1),
	(3, '01 4C 61 62', 3, 0, 0, 1),
	(4, '8A 8C 93 E6', 4, 0, 0, 1),
	(5, '8E 2E 03 79', 5, 0, 0, 1),
	(6, 'CA 89 D6 8B', 6, 0, 0, 1),
	(7, 'EF 22 A4 0B', 7, 0, 0, 1),
	(8, 'D8 B7 B4 8C', 8, 0, 1, 1),
	(9, '1C 2C E8 B2', 9, 0, 1, 1),
	(10, 'CA 2B AC A2', 0, 1, 0, 1),
	(11, 'D6 6D 36 9B', 1, 1, 1, 1),
	(12, 'F5 02 67 DC', 2, 1, 1, 1),
	(13, '62 9A 12 EC', 3, 1, 0, 1),
	(14, 'BA 5D C5 73', 4, 1, 1, 1),
	(15, 'FF C0 2E 82', 5, 1, 1, 1),
	(16, 'A3 4A 02 86', 6, 1, 0, 1),
	(17, '5D DC B9 64', 7, 1, 1, 1),
	(18, 'ED 2B B2 AB', 8, 1, 0, 1),
	(19, 'A6 34 65 10', 9, 1, 1, 1),
	(20, '75 6E 0D 0F', 0, 2, 0, 1),
	(21, 'AE F0 A6 58', 1, 2, 1, 1),
	(22, '53 A7 69 47', 2, 2, 1, 1),
	(23, 'C3 07 16 F5', 3, 2, 0, 1),
	(24, '4B A4 A1 59', 4, 2, 1, 1),
	(25, '36 81 80 C8', 5, 2, 1, 1),
	(26, '3A D8 C1 B6', 6, 2, 0, 1),
	(27, 'CA DB 9B 81', 7, 2, 1, 1),
	(28, '1C 97 36 B9', 8, 2, 0, 1),
	(29, 'B3 17 14 77', 9, 2, 0, 1),
	(30, 'D6 D3 D2 14', 0, 3, 0, 1),
	(31, '3B 00 03 72', 1, 3, 0, 1),
	(32, '5B EC E7 CB', 2, 3, 1, 1),
	(33, 'E6 99 AB 96', 3, 3, 0, 1),
	(34, 'B2 8B E1 09', 4, 3, 1, 1),
	(35, 'AA CA F9 87', 5, 3, 0, 1),
	(36, 'C3 90 3C 36', 6, 3, 1, 1),
	(37, 'DD F8 45 63', 7, 3, 0, 1),
	(38, 'AA 04 09 BD', 8, 3, 0, 1),
	(39, '77 1E E1 56', 9, 3, 0, 1),
	(40, 'EE 34 FA 27', 0, 4, 0, 1),
	(41, 'AB 45 50 95', 1, 4, 1, 1),
	(42, 'FB E1 BD 4D', 2, 4, 1, 1),
	(43, '9F 24 34 DA', 3, 4, 0, 1),
	(44, 'C8 3A E5 75', 4, 4, 1, 1),
	(45, 'E5 75 BC 3D', 5, 4, 1, 1),
	(46, 'A8 47 5D DE', 6, 4, 0, 1),
	(47, '61 16 2F 8E', 7, 4, 0, 1),
	(48, '3F 7C E6 5F', 8, 4, 0, 1),
	(49, '91 E0 DD E1', 9, 4, 1, 1),
	(50, 'DD FF B5 E6', 0, 5, 0, 1),
	(51, '58 C3 4D 02', 1, 5, 1, 1),
	(52, '6D 4C 6C E4', 2, 5, 1, 1),
	(53, '0A 7A 93 EA', 3, 5, 0, 1),
	(54, '0A 3D DB 42', 4, 5, 1, 1),
	(55, '14 45 9C 4A', 5, 5, 1, 1),
	(56, '1E EC 9E EB', 6, 5, 0, 1),
	(57, '52 83 A0 BF', 7, 5, 0, 1),
	(58, '07 92 E1 BB', 8, 5, 1, 1),
	(59, '54 09 90 D0', 9, 5, 1, 1),
	(60, '76 AF EA A3', 0, 6, 0, 1),
	(61, '56 D6 FA 41', 1, 6, 0, 1),
	(62, '0C 8E 78 5F', 2, 6, 0, 1),
	(63, '08 CE 8E 5F', 3, 6, 0, 1),
	(64, 'E2 97 D8 98', 4, 6, 0, 1),
	(65, 'E9 13 45 4F', 5, 6, 0, 1),
	(66, 'F8 4F 64 66', 6, 6, 0, 1),
	(67, 'C9 7B B8 E2', 7, 6, 0, 1),
	(68, 'A7 C9 52 2D', 8, 6, 0, 1),
	(69, 'C0 B4 9D 24', 9, 6, 1, 1),
	(70, '65 35 31 0A', 0, 7, 0, 1),
	(71, '5C FA 26 CD', 1, 7, 1, 1),
	(72, '53 F7 8F 39', 2, 7, 1, 1),
	(73, '9B BB B3 36', 3, 7, 0, 1),
	(74, 'D8 71 1F A4', 4, 7, 0, 1),
	(75, '81 0C 82 F7', 5, 7, 0, 1),
	(76, '6E 60 A6 5B', 6, 7, 0, 1),
	(77, '73 78 DE AF', 7, 7, 0, 1),
	(78, 'E5 BB 05 3D', 8, 7, 0, 1),
	(79, '9B C7 BF 4C', 9, 7, 0, 1),
	(80, 'E3 7F 14 14', 0, 8, 1, 1),
	(81, 'AB 5F 1A 66', 1, 8, 0, 1),
	(82, 'A9 A3 7D 91', 2, 8, 0, 1),
	(83, '51 98 E7 F2', 3, 8, 0, 1),
	(84, '75 62 86 AD', 4, 8, 0, 1),
	(85, 'D5 D5 1B 83', 5, 8, 1, 1),
	(86, '3C 61 7B AC', 6, 8, 0, 1),
	(87, '71 38 56 D1', 7, 8, 0, 1),
	(88, '41 DE 4F D8', 8, 8, 0, 1),
	(89, 'D2 39 21 22', 9, 8, 1, 1);
/*!40000 ALTER TABLE `map` ENABLE KEYS */;

-- Dumping structure for table agv.pod
CREATE TABLE IF NOT EXISTS `pod` (
  `id` smallint(6) NOT NULL DEFAULT 0,
  `product_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `node_id` smallint(6) DEFAULT NULL,
  `load` smallint(3) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `node_id` (`node_id`),
  KEY `FK2_product_id` (`product_id`),
  CONSTRAINT `FK1_pod_nodeId` FOREIGN KEY (`node_id`) REFERENCES `map` (`id`),
  CONSTRAINT `FK2_product_id` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table agv.pod: ~21 rows (approximately)
DELETE FROM `pod`;
/*!40000 ALTER TABLE `pod` DISABLE KEYS */;
INSERT INTO `pod` (`id`, `product_id`, `quantity`, `node_id`, `load`) VALUES
	(0, 2, NULL, 0, 0),
	(1, 1, NULL, 12, 5),
	(2, 1, NULL, 25, 45),
	(3, 5, NULL, 55, 65),
	(4, 4, NULL, 51, 67),
	(5, 1, NULL, 42, 89),
	(6, 4, NULL, 41, 78),
	(7, 5, NULL, 44, 78),
	(8, 4, NULL, 45, 0),
	(9, 5, NULL, 71, 0),
	(10, 3, NULL, 54, 0),
	(11, 5, NULL, 72, 0),
	(12, 2, NULL, 11, 5),
	(13, 5, NULL, 52, 0),
	(14, 5, NULL, 15, 45),
	(15, 5, NULL, 14, 43),
	(16, 3, NULL, 17, 23),
	(17, 2, NULL, 27, 12),
	(18, 4, NULL, 21, 5),
	(19, 3, NULL, 22, 23),
	(20, 4, NULL, 24, 43);
/*!40000 ALTER TABLE `pod` ENABLE KEYS */;

-- Dumping structure for table agv.products
CREATE TABLE IF NOT EXISTS `products` (
  `id` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `info` char(50) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `import_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Dumping data for table agv.products: ~5 rows (approximately)
DELETE FROM `products`;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` (`id`, `name`, `info`, `quantity`, `import_date`) VALUES
	(1, 'toy', 'test1', 100, '2021-06-04 16:55:37'),
	(2, 'pen', 'test1', 56, '2021-06-04 16:55:37'),
	(3, 'pencil', 'test1', 56, '2021-06-04 16:55:37'),
	(4, 'iphone 12', 'Not included charger', 1, '2021-06-04 16:55:37'),
	(5, 'iphone 12 charger', 'Super fast charger', 5, '2021-06-04 16:55:37');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;

-- Dumping structure for table agv.station
CREATE TABLE IF NOT EXISTS `station` (
  `id` smallint(6) NOT NULL DEFAULT 0,
  `node_id` smallint(6) NOT NULL DEFAULT 0,
  `available` tinyint(4) DEFAULT 1,
  PRIMARY KEY (`id`),
  UNIQUE KEY `node_id` (`node_id`),
  CONSTRAINT `FK1_station_nodeId` FOREIGN KEY (`node_id`) REFERENCES `map` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table agv.station: ~0 rows (approximately)
DELETE FROM `station`;
/*!40000 ALTER TABLE `station` DISABLE KEYS */;
INSERT INTO `station` (`id`, `node_id`, `available`) VALUES
	(1, 59, 1);
/*!40000 ALTER TABLE `station` ENABLE KEYS */;

-- Dumping structure for table agv.station_log
CREATE TABLE IF NOT EXISTS `station_log` (
  `order_id` smallint(5) NOT NULL,
  `station_id` smallint(5) DEFAULT NULL,
  `type` varchar(50) DEFAULT NULL,
  `agv_id` smallint(5) DEFAULT NULL,
  `pod_id` smallint(6) DEFAULT NULL,
  `result` smallint(5) DEFAULT 1,
  `describe` varchar(50) DEFAULT NULL,
  `time` datetime DEFAULT NULL,
  PRIMARY KEY (`order_id`),
  UNIQUE KEY `order_id` (`order_id`),
  KEY `FK1_station_id` (`station_id`),
  KEY `FK2_agv_id` (`agv_id`),
  KEY `FK3_pod_id` (`pod_id`),
  CONSTRAINT `FK1_station_id` FOREIGN KEY (`station_id`) REFERENCES `station` (`id`),
  CONSTRAINT `FK2_agv_id` FOREIGN KEY (`agv_id`) REFERENCES `agv` (`id`),
  CONSTRAINT `FK3_pod_id` FOREIGN KEY (`pod_id`) REFERENCES `pod` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Dumping data for table agv.station_log: ~0 rows (approximately)
DELETE FROM `station_log`;
/*!40000 ALTER TABLE `station_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `station_log` ENABLE KEYS */;

-- Dumping structure for table agv.waiting_zone
CREATE TABLE IF NOT EXISTS `waiting_zone` (
  `id` smallint(6) NOT NULL DEFAULT 1,
  `node_id` smallint(6) NOT NULL DEFAULT 0,
  `in_quere` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `node_id` (`node_id`),
  CONSTRAINT `FK1_waiting_zone_node_id` FOREIGN KEY (`node_id`) REFERENCES `map` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Dumping data for table agv.waiting_zone: ~3 rows (approximately)
DELETE FROM `waiting_zone`;
/*!40000 ALTER TABLE `waiting_zone` DISABLE KEYS */;
INSERT INTO `waiting_zone` (`id`, `node_id`, `in_quere`) VALUES
	(1, 49, 0),
	(2, 58, 0),
	(3, 69, 0);
/*!40000 ALTER TABLE `waiting_zone` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
